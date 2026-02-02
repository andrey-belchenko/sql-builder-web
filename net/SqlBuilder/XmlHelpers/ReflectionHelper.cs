using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using sql.builder.DataApi;
using sql.builder.UI;

namespace sql.builder.XmlHelpers
{
    public static class ReflectionHelper
    {
        private static string[] assemblies = new string[] {
            "arbitrage.lib", "asuse.Net", "finance.Net", "sql.builder"
        };
        private static void ResolveAssemblyName(string type_name, ref string assembly_name)
        {
            if (string.IsNullOrEmpty(assembly_name)) {
                for (int index = 0; index < assemblies.Length; index++) {
                    assembly_name = assemblies[index];
                    if (type_name.StartsWith(assembly_name)) {
                        return;
                    }
                }
                assembly_name = "mscorlib";
            }
        }
        public static void ResolveAssemblyType(string assembly_name, string type_name, out Type type)
        {
            Contract.Assume(!string.IsNullOrEmpty(type_name));
            // Если сборка не указана, пытаемся её определить по имени класса
            ResolveAssemblyName(type_name, ref assembly_name);
            AssemblyName an = new AssemblyName(assembly_name);
            Assembly assembly = AppDomain.CurrentDomain.Load(an);
            Contract.Assert(assembly != null);
            Debug.WriteLine("Загружена сборка " + assembly.FullName);
            type = assembly.GetType(type_name, false);
            if (type != null) {
                Debug.WriteLine("Найден тип " + type.FullName);
            }
        }
        public static object ExecuteStaticMethod(string assembly_name, string type_name, string method_name, IList<object> input_params, DataSet ds)
        {
            Contract.Assume(!string.IsNullOrEmpty(type_name));
            Contract.Assume(!string.IsNullOrEmpty(method_name));
            Contract.Assume(input_params != null);
            #if DEBUG
            Debug.Write("Пытаемся вызвать статический метод ");
            if (!string.IsNullOrEmpty(assembly_name)) {
                Debug.Write("[");
                Debug.Write(assembly_name);
                Debug.Write("]");
            }
            Debug.WriteLine(type_name + "::" + method_name + "() ...");
            #endif
            Type type;
            ResolveAssemblyType(assembly_name, type_name, out type);
            if (type == null) {
                throw new InvalidOperationException("Не удалось найти тип " + type_name);
            }
            MethodInfo method_info = type.GetMethod(method_name, BindingFlags.Static | BindingFlags.Public); // Даёт AmbiguousMatchException в случае перегруженного метода
            Contract.Assert(method_info != null);
            Debug.WriteLine("Найден метод " + method_info.ToString());
            ParameterInfo[] pi = method_info.GetParameters();
            int par_count = pi.Length;
            //
            Contract.Assert(par_count >= input_params.Count);
            object[] parameters = new object[par_count];
            int index = 0;
            while (index < input_params.Count) {
                Type param_type = pi[index].ParameterType;
                object value = input_params[index];
                if (value != null && (Convert.IsDBNull(value) || Cmn.undefinedString == value.ToString())) {
                    value = null;
                }
                if (value != null) {
                    if (param_type.IsGenericType && param_type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                        // для преобразований типа decimal => decimal? и decimal => float?
                        param_type = param_type.GetGenericArguments()[0];
                    }
                    if (param_type != value.GetType()) {
                        if (param_type.IsEnum && value is string) {
                            // для преобразований строк в соотв. перечисления, например, "YesNo" => MessageBoxButton.YesNo
                            value = Enum.Parse(param_type, (string)value);
                        } else if (value is IConvertible) {
                            // для преобразований decimal => bool, decimal => int, "true" и "false" => bool etc
                            value = Convert.ChangeType(value, param_type);
                        }
                    }
                }
                parameters[index] = value;
                index++;
            }
            while (index < par_count) {
                ParameterInfo param_info = pi[index];
                if (param_info.ParameterType == typeof(DataSet)) {
                    parameters[index] = ds;
                } else if (param_info.IsOptional && param_info.HasDefaultValue) {
                    parameters[index] = param_info.DefaultValue;
                }
                index++;
            }
            object return_value = null;
            try {
                return_value = method_info.Invoke(null, parameters);
            } catch (TargetInvocationException ex) {
                Exception inner_exception = ex.InnerException;
                Debug.WriteLine("При вызове воникла ошибка: " + inner_exception.GetType().FullName);
                Debug.WriteLine("Стек вызова:");
                Debug.WriteLine(inner_exception.StackTrace);
                throw inner_exception;
            }
            #if DEBUG
            if (method_info.ReturnType != typeof(void)) {
                Debug.Write("  Возвращаемое значение: " + method_info.ToString());
                if (return_value != null) {
                    Debug.WriteLine(return_value.ToString());
                } else {
                    Debug.WriteLine("null");
                }
            }
            #endif
            return return_value;
        }
    }
}