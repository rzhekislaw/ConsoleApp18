namespace ns
{
    class tClass
    {
        public static void Main()
        {
            var o = new Model();
            GetConstructorsInfo(o);
            GetMethodsInfo(o);
            GetFieldsInfo(o);
            GetPropertiesInfo(o);

            //var obj = CreateObject<Model>();
            //InvokeMethods(obj);
            //GetFieldsValues(obj);
            //GetPropertiesValues(obj);
        }

        #region GetInfo
        public static void GetConstructorsInfo<T>(T obj) where T : class
        {
            Console.WriteLine("Constructors:");
            Console.WriteLine();
            var publics = typeof(T).GetConstructors().Where(w => w.IsPublic == true);

            foreach (var conctructor in publics)
            {
                var parameters = conctructor.GetParameters().ToDictionary(pr => pr.Name, pr => pr.ParameterType.ToString().ToLower().Substring(pr.ParameterType.ToString().LastIndexOf('.') + 1));
                string parametersToShow = "";

                if (parameters.Count() > 0)
                {
                    foreach (var param in parameters)
                    {
                        parametersToShow += param.Value + " " + param.Key + ", ";
                    }
                    parametersToShow = parametersToShow.Substring(0, parametersToShow.LastIndexOf(','));
                }
                Console.WriteLine($"public {typeof(T).ToString().Substring(typeof(T).ToString().LastIndexOf('.') + 1)}({parametersToShow})");
            }

            Console.WriteLine();

            var privates = typeof(T).GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(w => w.IsPrivate == true);

            foreach (var conctructor in privates)
            {
                var parameters = conctructor.GetParameters().ToDictionary(pr => pr.Name, pr => pr.ParameterType.ToString().ToLower().Substring(pr.ParameterType.ToString().LastIndexOf('.') + 1));
                string parametersToShow = "";

                if (parameters.Count() > 0)
                {
                    foreach (var param in parameters)
                    {
                        parametersToShow += param.Value + " " + param.Key + ", ";
                    }
                    parametersToShow = parametersToShow.Substring(0, parametersToShow.LastIndexOf(','));
                }
                Console.WriteLine($"private {typeof(T).ToString().Substring(typeof(T).ToString().LastIndexOf('.') + 1)}({parametersToShow})");
            }
            Console.WriteLine();
        }

        public static void GetMethodsInfo<T>(T obj) where T : class
        {
            Console.WriteLine("Methods:");
            Console.WriteLine();
            var publics = typeof(T).GetMethods().Where(w => !typeof(object).GetMethods().Select(s => s.Name).Contains(w.Name) && w.IsPublic == true && !typeof(T).GetProperties().Select(s => s.GetGetMethod()).Contains(w) && !typeof(T).GetProperties().Select(s => s.GetSetMethod()).Contains(w));

            foreach (var method in publics)
            {
                var parameters = method.GetParameters().ToDictionary(pr => pr.Name, pr => pr.ParameterType.ToString().ToLower().Substring(method.ReturnType.ToString().LastIndexOf('.') + 1));
                string parametersToShow = "";
                if (parameters.Count() > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        parametersToShow += parameter.Value + " " + parameter.Key + ", ";
                    }
                    parametersToShow = parametersToShow.Substring(0, parametersToShow.LastIndexOf(','));
                }
                Console.WriteLine($"public {(method.IsStatic ? "static " : "")}{method.ReturnType.ToString().ToLower().Substring(method.ReturnType.ToString().LastIndexOf('.') + 1)} {method.Name}({parametersToShow})");
            }

            Console.WriteLine();

            var privates = typeof(T).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static).Where(w => !typeof(object).GetMethods().Select(s => s.Name).Contains(w.Name) && w.IsPrivate == true && !typeof(T).GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static).Select(s => s.GetGetMethod(true)).Contains(w) && !typeof(T).GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static).Select(s => s.GetSetMethod(true)).Contains(w));

            foreach (var method in privates)
            {
                var parameters = method.GetParameters().ToDictionary(pr => pr.Name, pr => pr.ParameterType.ToString().ToLower().Substring(method.ReturnType.ToString().LastIndexOf('.') + 1));
                string parametersToShow = "";
                if (parameters.Count() > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        parametersToShow += parameter.Value + " " + parameter.Key + ", ";
                    }
                    parametersToShow = parametersToShow.Substring(0, parametersToShow.LastIndexOf(','));
                }
                Console.WriteLine($"private {(method.IsStatic ? "static " : "")}{method.ReturnType.ToString().ToLower().Substring(method.ReturnType.ToString().LastIndexOf('.') + 1)} {method.Name}({parametersToShow})");
            }
            Console.WriteLine();
        }

        public static void GetFieldsInfo<T>(T obj) where T : class
        {
            Console.WriteLine("Fields:");
            Console.WriteLine();

            var publics = typeof(T).GetFields().Where(w => w.IsPublic == true);

            foreach (var field in publics)
            {
                Console.WriteLine($"public {field.FieldType.ToString().ToLower().Substring(field.FieldType.ToString().LastIndexOf('.') + 1)} {field.Name}");
            }

            Console.WriteLine();

            var privates = typeof(T).GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(w => w.IsPrivate == true && w.CustomAttributes.Count() == 0);

            foreach (var field in privates)
            {
                Console.WriteLine($"private {field.FieldType.ToString().ToLower().Substring(field.FieldType.ToString().LastIndexOf('.') + 1)} {field.Name}");
            }
            Console.WriteLine();
        }

        public static void GetPropertiesInfo<T>(T obj) where T : class
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("PropertiesInfo shows only if getter or setter exists ignoring its definition (default / overridden)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Properties:");
            Console.WriteLine();

            var publics = typeof(T).GetProperties();

            foreach (var property in publics)
            {
                Console.WriteLine($"public {property.PropertyType.ToString().ToLower().Substring(property.PropertyType.ToString().LastIndexOf('.') + 1)} {property.Name} {{ {(property.GetMethod == null ? "" : "get;")}{(property.SetMethod == null ? "" : " set;")} }}");
            }

            Console.WriteLine();

            var privates = typeof(T).GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);

            foreach (var property in privates)
            {
                Console.WriteLine($"private {property.PropertyType.ToString().ToLower().Substring(property.PropertyType.ToString().LastIndexOf('.') + 1)} {property.Name} {{ {(property.GetMethod == null ? "" : "get;")}{(property.SetMethod == null ? "" : " set;")} }}");
            }

            Console.WriteLine();
        }

        #endregion


        public static T CreateObject<T>() where T : new()
        {
            Console.WriteLine();
            Console.WriteLine("Creating object");
            var obj = typeof(T).GetConstructors().ElementAt(0).Invoke(null);
            return (T)obj;
        }

        public static void InvokeMethods<T>(T obj)
        {
            Console.WriteLine();
            Console.WriteLine("Invoking methods");
            var noParamMethods = typeof(T).GetMethods().Where(w => !typeof(object).GetMethods().Select(s => s.Name).Contains(w.Name) && w.IsPublic == true && !typeof(T).GetProperties().Select(s => s.GetGetMethod()).Contains(w) && !typeof(T).GetProperties().Select(s => s.GetSetMethod()).Contains(w) && w.GetParameters().Count() == 0);

            foreach (var method in noParamMethods)
            {
                method.Invoke(obj, null);
            }
        }

        public static void GetFieldsValues<T>(T obj)
        {
            Console.WriteLine();
            Console.WriteLine("Getting fields` values");
            var publics = typeof(T).GetFields().Where(w => w.IsPublic == true).ToList();
            var privates = typeof(T).GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(w => w.IsPrivate == true && w.CustomAttributes.Count() == 0).ToList();
            var fields = new List<System.Reflection.FieldInfo>();
            fields.AddRange(publics);
            fields.AddRange(privates);

            foreach (var field in fields)
            {
                Console.WriteLine(field.Name + " - " + field.GetValue(obj) ?? "value was not set");
            }
        }

        public static void GetPropertiesValues<T>(T obj)
        {
            Console.WriteLine();
            Console.WriteLine("Getting properties` values");
            var publics = typeof(T).GetProperties();
            var privates = typeof(T).GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
            var properties = new List<System.Reflection.PropertyInfo>();
            properties.AddRange(publics);
            properties.AddRange(privates);

            foreach (var property in properties)
            {
                Console.WriteLine(property.Name + " - " + property.GetValue(obj) ?? "value was not set");
            }
        }
    }

    public class Model
    {
        public Model() { }

        public Model(int num)
        {
            this.num = num;
        }

        private Model(object val_1, object val_2)
        {

        }

        public void Show()
        {
            Console.WriteLine(name ?? "name was not set" + " - " + num);
        }

        public void Update(int val)
        {
            upd(val);
        }
        private void upd(int val)
        {
            num += val;
        }

        private static void m()
        {

        }

        public int number;

        private int num;

        public string fullname { get; }

        private string name { get; set; }
    }
}