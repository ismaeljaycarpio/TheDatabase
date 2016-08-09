using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using System.CodeDom;
using System.Text;
using System.Drawing;
using System.IO;
/// <summary>
/// Summary description for ETSMath
/// </summary>
public class ETSMath
{
	public ETSMath()
	{
		//
		// TODO: Add constructor logic here
		//
        //GetMathMemberNames(); 
	}




        public static decimal Evaluate(string MathExpression)
        {
            decimal result = 0;
            try
            {
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateExecutable = false;
                compilerParameters.GenerateInMemory = false;

                string tmpModuleSource = "namespace ns{"
                                    + "using System;"
                                    + "class class1{"
                                    + "    public static decimal Eval(){"
                                    +"          return Convert.ToDecimal("+ MathExpression + ");"
                                    +"     }"
                                    +"}} ";

                CompilerResults compilerResults = codeProvider.CompileAssemblyFromSource(
                                                  compilerParameters, tmpModuleSource);
                if (compilerResults.Errors.Count > 0)
                {
                        //If a compiler error is generated, we will throw an exception because
                        //the syntax was wrong - again, this is left up to the implementer to verify
                        //syntax before calling the function.  The calling code could trap this in a
                        //try loop, and notify a user
                        //the command was not understood, for example.
                    throw new ArgumentException("Expression cannot be evaluated [" + MathExpression + "]");
                }
                else
                {
                    MethodInfo Methinfo = compilerResults.CompiledAssembly.GetType("ns.class1").GetMethod("Eval");
                    result = (decimal)Methinfo.Invoke(null, null);
                }
            }
            catch (Exception Ex)
            {
                throw new ArgumentException("Expression cannot be evaluated, please use a valid C# expression [" + MathExpression + "] -- Ex[" + Ex.Message + "][" + Ex.InnerException + "]");
            }
            return result;
        }







         

            public void CodedomCalculator()
            {
                //
                // Required for Windows Form Designer support
                //
                //InitializeComponent();

                //GetMathMemberNames();  // track all members of the math namespace

                //
                // TODO: Add any constructor code after InitializeComponent call
                //
            }

          

            

          static  ICodeCompiler CreateCompiler()
            {
                //Create an instance of the C# compiler   
                CodeDomProvider codeProvider = null;
                codeProvider = new CSharpCodeProvider();
                ICodeCompiler compiler = codeProvider.CreateCompiler();
                return compiler;
            }

            /// <summary>
            /// Creawte parameters for compiling
            /// </summary>
            /// <returns></returns>
          static  CompilerParameters CreateCompilerParameters()
            {
                //add compiler parameters and assembly references
                CompilerParameters compilerParams = new CompilerParameters();
                compilerParams.CompilerOptions = "/target:library /optimize";
                compilerParams.GenerateExecutable = false;
                compilerParams.GenerateInMemory = true;
                compilerParams.IncludeDebugInformation = false;
                compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
                compilerParams.ReferencedAssemblies.Add("System.dll");
                compilerParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");

                //add any aditional references needed
                //            foreach (string refAssembly in code.References)
                //              compilerParams.ReferencedAssemblies.Add(refAssembly);

                return compilerParams;
            }

          

            /// <summary>
            /// Compiles the code from the code string
            /// </summary>
            /// <param name="compiler"></param>
            /// <param name="parms"></param>
            /// <param name="source"></param>
            /// <returns></returns>
            private static CompilerResults CompileCode(ICodeCompiler compiler, CompilerParameters parms, string source)
            {
                //actually compile the code
                CompilerResults results = compiler.CompileAssemblyFromSource(
                                            parms, source);

                //Do we have any compiler errors?
                if (results.Errors.Count > 0)
                {
                    foreach (CompilerError error in results.Errors)
                        //WriteLine("Compile Error:" + error.ErrorText);
                    return null;
                }

                return results;
            }

            /// <summary>
            /// Need to change eval string to use .NET Math library
            /// </summary>
            /// <param name="eval">evaluation expression</param>
            /// <returns></returns>
           public static  string RefineEvaluationString(string eval)
            {
                //eval = "(" + eval + ")*1.0";
                GetMathMemberNames();
                // look for regular expressions with only letters
                Regex regularExpression = new Regex("[a-zA-Z_]+");

                // track all functions and constants in the evaluation expression we already replaced
                ArrayList replacelist = new ArrayList();

                // find all alpha words inside the evaluation function that are possible functions
                MatchCollection matches = regularExpression.Matches(eval);
                foreach (Match m in matches)
                {
                    // if the word is found in the math member map, add a Math prefix to it
                    bool isContainedInMathLibrary = _mathMembersMap[m.Value.ToUpper()] != null;
                    if (replacelist.Contains(m.Value) == false && isContainedInMathLibrary)
                    {
                        eval = eval.Replace(m.Value, "Math." + _mathMembersMap[m.Value.ToUpper()]);
                    }

                    // we matched it already, so don't allow us to replace it again
                    replacelist.Add(m.Value);
                }

                // return the modified evaluation string
                return eval;
            }

          

            /// <summary>
            /// Compiles the c# into an assembly if there are no syntax errors
            /// </summary>
            /// <returns></returns>
            public static CompilerResults CompileAssembly()
            {
                // create a compiler
                ICodeCompiler compiler = CreateCompiler();
                // get all the compiler parameters
                CompilerParameters parms = CreateCompilerParameters();
                // compile the code into an assembly
                CompilerResults results = CompileCode(compiler, parms, _source.ToString());
                return results;

            }

         
            static ArrayList _mathMembers = new ArrayList();
           static Hashtable _mathMembersMap = new Hashtable();

            static void GetMathMemberNames()
            {
                // get a reflected assembly of the System assembly
                Assembly systemAssembly = Assembly.GetAssembly(typeof(System.Math));
                try
                {
                    //cant call the entry method if the assembly is null
                    if (systemAssembly != null)
                    {
                        //Use reflection to get a reference to the Math class

                        Module[] modules = systemAssembly.GetModules(false);
                        Type[] types = modules[0].GetTypes();

                        //loop through each class that was defined and look for the first occurrance of the Math class
                        foreach (Type type in types)
                        {
                            if (type.Name == "Math")
                            {
                                // get all of the members of the math class and map them to the same member
                                // name in uppercase
                                MemberInfo[] mis = type.GetMembers();
                                foreach (MemberInfo mi in mis)
                                {
                                    _mathMembers.Add(mi.Name);
                                    _mathMembersMap[mi.Name.ToUpper()] = mi.Name;
                                }
                            }
                            //if the entry point method does return in Int32, then capture it and return it
                        }


                        //if it got here, then there was no entry point method defined.  Tell user about it
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:  An exception occurred while executing the script", ex);
                }
            }

            /// <summary>
            /// Runs the Calculate method in our on-the-fly assembly
            /// </summary>
            /// <param name="results"></param>
            public static string RunCode(CompilerResults results)
            {
                Assembly executingAssembly = results.CompiledAssembly;
                try
                {
                    //cant call the entry method if the assembly is null
                    if (executingAssembly != null)
                    {
                        object assemblyInstance = executingAssembly.CreateInstance("ExpressionEvaluator.Calculator");
                        //Use reflection to call the static Main function

                        Module[] modules = executingAssembly.GetModules(false);
                        Type[] types = modules[0].GetTypes();

                        //loop through each class that was defined and look for the first occurrance of the entry point method
                        foreach (Type type in types)
                        {
                            MethodInfo[] mis = type.GetMethods();
                            foreach (MethodInfo mi in mis)
                            {
                                if (mi.Name == "Calculate")
                                {
                                    object result = mi.Invoke(assemblyInstance, null);
                                    return result.ToString();
                                }
                            }
                        }

                    }
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:  An exception occurred while executing the script", ex);
                }

                return "";

            }


          static  CodeMemberField FieldVariable(string fieldName, string typeName, MemberAttributes accessLevel)
            {
                CodeMemberField field = new CodeMemberField(typeName, fieldName);
                field.Attributes = accessLevel;
                return field;
            }
          static  CodeMemberField FieldVariable(string fieldName, Type type, MemberAttributes accessLevel)
            {
                CodeMemberField field = new CodeMemberField(type, fieldName);
                field.Attributes = accessLevel;
                return field;
            }

            /// <summary>
            /// Very simplistic getter/setter properties
            /// </summary>
            /// <param name="propName"></param>
            /// <param name="internalName"></param>
            /// <param name="type"></param>
            /// <returns></returns>
           static CodeMemberProperty MakeProperty(string propertyName, string internalName, Type type)
            {
                CodeMemberProperty myProperty = new CodeMemberProperty();
                myProperty.Name = propertyName;
                myProperty.Comments.Add(new CodeCommentStatement(String.Format("The {0} property is the returned result", propertyName)));
                myProperty.Attributes = MemberAttributes.Public;
                myProperty.Type = new CodeTypeReference(type);
                myProperty.HasGet = true;
                myProperty.GetStatements.Add(
                    new CodeMethodReturnStatement(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), internalName)));

                myProperty.HasSet = true;
                myProperty.SetStatements.Add(
                    new CodeAssignStatement(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), internalName),
                        new CodePropertySetValueReferenceExpression()));

                return myProperty;
            }


           static StringBuilder _source = new StringBuilder();

            /// <summary>
            /// Main driving routine for building a class
            /// </summary>
          public static  void BuildClass(string expression)
            {
                // need a string to put the code into
                _source = new StringBuilder();
                StringWriter sw = new StringWriter(_source);

                //Declare your provider and generator
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                ICodeGenerator generator = codeProvider.CreateGenerator(sw);
                CodeGeneratorOptions codeOpts = new CodeGeneratorOptions();

                CodeNamespace myNamespace = new CodeNamespace("ExpressionEvaluator");
                myNamespace.Imports.Add(new CodeNamespaceImport("System"));
               // myNamespace.Imports.Add(new CodeNamespaceImport("System.Windows.Forms"));

                //Build the class declaration and member variables			
                CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration();
                classDeclaration.IsClass = true;
                classDeclaration.Name = "Calculator";
                classDeclaration.Attributes = MemberAttributes.Public;
                classDeclaration.Members.Add(FieldVariable("answer", typeof(double), MemberAttributes.Private));

                //default constructor
                CodeConstructor defaultConstructor = new CodeConstructor();
                defaultConstructor.Attributes = MemberAttributes.Public;
                defaultConstructor.Comments.Add(new CodeCommentStatement("Default Constructor for class", true));
                defaultConstructor.Statements.Add(new CodeSnippetStatement("//TODO: implement default constructor"));
                classDeclaration.Members.Add(defaultConstructor);

                //property
                classDeclaration.Members.Add(MakeProperty("Answer", "answer", typeof(double)));

                //Our Calculate Method
                CodeMemberMethod myMethod = new CodeMemberMethod();
                myMethod.Name = "Calculate";
                myMethod.ReturnType = new CodeTypeReference(typeof(double));
                myMethod.Comments.Add(new CodeCommentStatement("Calculate an expression", true));
                myMethod.Attributes = MemberAttributes.Public;
                myMethod.Statements.Add(new CodeAssignStatement(new CodeSnippetExpression("Answer"), new CodeSnippetExpression(expression)));
                //            myMethod.Statements.Add(new CodeSnippetExpression("MessageBox.Show(String.Format(\"Answer = {0}\", Answer))"));
                myMethod.Statements.Add(
                    new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Answer")));
                classDeclaration.Members.Add(myMethod);
                //write code
                myNamespace.Types.Add(classDeclaration);
                generator.GenerateCodeFromNamespace(myNamespace, sw, codeOpts);
                sw.Flush();
                sw.Close();
            }

        





}