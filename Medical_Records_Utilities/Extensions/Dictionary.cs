using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Medical_Records_Utilities.Extensions
{
    public class Dictionary
    {

        private static readonly Dictionary<string, string> ExpresionesRegularesInformacionPersonal = new Dictionary<string, string>
        {
            {"CEDULA", @"^[0-9]{1,25}$"},
            {"NOMBRE", @"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$"},
            {"APELLIDOS", @"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$"},
            {"EMAIL", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"},

            {"EDAD", @"^[0-9]{1,2}$"},
            {"SEXO", @"^(?:Masculino|Femenino|Otro){1,50}$"},
            {"DIRECCION", "^.{0,}$"},
            {"LUGAR_ORIGEN", @"^[áÁéÉíÍóÓúÚa-zA-Z\s]{0,100}$"},
            {"TELEFONO", @"^\d{1,20-}$"},
            {"PROFESION", "^.{0,300}$"},
            {"ESTADO_CIVIL", @"^[a-zA-Z\s]{1,50}$"},
            {"CONDICION_REHABILITACION", @"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,50}$"},
            {"FASE", @"^[a-zA-Z0-9\s]{1,10}$"},
            {"NUMERO_EXPEDIENTE", @"^[a-zA-Z0-9-\s]{0,100}$"}
        };

        //public static Dictionary<string, bool> ValidarTodosLosCampos(Dictionary<string, string> campos)
        //{
        //    var resultados = new Dictionary<string, bool>();

        //    foreach (var campo in campos)
        //    {
        //        if (ExpresionesRegulares.TryGetValue(campo.Key, out string expresionRegular))
        //        {
        //            bool esValido = Regex.IsMatch(campo.Value, expresionRegular);
        //            resultados[campo.Key] = esValido;
        //        }
        //        else
        //        {
        //            // Si el nombre del campo no se encuentra en el diccionario, asumimos que es válido.
        //            resultados[campo.Key] = true;
        //        }
        //    }

        //    return resultados;
        //}


        public bool ValidarTodosLosCampos(Dictionary<string, string> campos)
        {
            var resultados = new Dictionary<string, bool>();

            foreach (var campo in campos)
            {
                if (ExpresionesRegularesInformacionPersonal.TryGetValue(campo.Key, out string expresionRegular))
                {
                    bool esValido = Regex.IsMatch(campo.Value, expresionRegular);
                    if (!esValido)
                        return false;
                }
            }

            return true;
        }
    }
}
