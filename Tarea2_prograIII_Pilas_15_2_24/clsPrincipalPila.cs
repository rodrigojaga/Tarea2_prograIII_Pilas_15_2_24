using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Calculus;

namespace Tarea2_prograIII_Pilas_15_2_24
{
    public class clsPrincipalPila
    {        
        static Queue<string> queNumbers = new Queue<string>();       
        
        static void Main(string[] args)
        {

            mtdStarterFuction();

        }

        /// <summary>
        /// Metodo utilizado para iniciar todo el flujo del programa
        /// Hace las verificaciones necesarias para saber que respuestas dar al usurio 
        /// luego de recibir su expresion matematica
        /// </summary>
        private static void mtdStarterFuction()
        {
            Console.WriteLine("Ingrese una expresion matematica con parentesis: ");

            string a = Console.ReadLine().Replace(" ","");

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;

            if (fncVerifyExpressionByRegex(a))
            {
                Console.WriteLine($"'{a}' Es una expresion matematica");
            }
            else
            {
                Console.WriteLine($"'{a}' No es una expresion matematica");
            }


            mtdTemporal(a);

            if (fncVerificarParentesis(a))
            {
                Console.WriteLine($"'{a}' Tiene parentesis de apertura y cierre");

            }
            else
            {
                Console.WriteLine($"'{a}' NO tiene parentesis de apertura y cierre");
            }
            

            fncOperacion(a);
            Console.WriteLine(fncGetLastCharacter(a));

            Console.ForegroundColor = ConsoleColor.White;

        }

        /// <summary>
        /// Por el momento, este metodo no tiene una funcionalidad real,
        /// fue creado unicamente para entender de mejor forma como es que
        /// funcionan las colas
        /// --------------------------------------------
        /// Metodo que obtiene la expresion matematica ingresada por el usuario 
        /// para luego meterla en una cola  
        /// </summary>
        /// <param name="strTextString"></param>
        private static void mtdTemporal(string strTextString)
        {

            Queue<char> queExpression = new Queue<char>();

            foreach (char chrCharacter in strTextString)
            {

                queExpression.Enqueue(chrCharacter);

            }

            fncVerifyExpression(queExpression);

        }

        /// <summary>
        /// Este metodo toma la expresion matematica ingresada por el usuario y la mete
        /// en una pila. Utiliza el metodo Peek, para decirle al usuario con que caracter
        /// fue que termino su expresion
        /// </summary>
        /// <param name="strTextString"></param>
        /// <returns></returns>
        private static string fncGetLastCharacter(string strTextString)
        {

            Stack<char> stkExpression = new Stack<char>();

            foreach (char chrCharacter in strTextString)
            {

                stkExpression.Push(chrCharacter);

            }

            return $"La operacion ingresada tiene como ultimo caracter '{stkExpression.Peek().ToString()}'";

        }

        /// <summary>
        /// Por el momento, este metodo no tiene una funcionalidad real,
        /// fue creado unicamente para entender de mejor forma como es que
        /// funcionan las colas
        /// --------------------------------------------
        /// Este metodo toma la cola con los caracteres de la expresion
        /// ingresada por el usuario para luego analizarla y separar numeros
        /// de los signos de la expresion:
        /// 
        /// por ejemplo si tenemos la expresion '5/(74+654*789-4455)'
        /// lo que nos devolvera este metodo es una cola asi:
        /// 5
        /// /
        /// (
        /// 74
        /// +
        /// 654
        /// *
        /// 789
        /// -
        /// 4455
        /// )        
        /// </summary>
        /// <param name="queTemp"></param>
        private static void fncVerifyExpression(Queue<char> queTemp)
        {
            int intLeghtNumber = 0;
            StringBuilder stbTempNumber = new StringBuilder();

            foreach (char chrCharacter in queTemp)
            {
                if (fncVerifyIfDigit(chrCharacter))
                {
                    stbTempNumber.Append(chrCharacter);
                }
                
                else
                {                    
                    queNumbers.Enqueue(stbTempNumber.ToString());
                    queNumbers.Enqueue(chrCharacter.ToString());
                    intLeghtNumber = stbTempNumber.ToString().Length;
                    break;
                }
            }

            while (intLeghtNumber >= 0)
            {
                queTemp.Dequeue();
                intLeghtNumber--;
            }            
            //Recursividad para que el proceso se repita una y otra vez
            //hasta que ya no hayan elementos en la cola
            if (queTemp.Count > 0)
            {
                fncVerifyExpression(queTemp);
            }
            
        }

        /// <summary>
        /// Esta funcion evalua que un caracter sea un digito o algo diferente a un digito
        /// 
        /// En caso de ser un digito devuelve true
        /// En caso de NO ser un digito devuelve false
        /// </summary>
        /// <param name="chrDigit"></param>
        /// <returns></returns>
        private static bool fncVerifyIfDigit(char chrDigit)
        {
            return (char.IsDigit(chrDigit)) ? true: false;
        }

        /// <summary>
        /// Este metodo resuelve la expresion matematica ingresada 
        /// Se utilizo la libreria 'Calculus.dll' hecha por johnspice
        /// 
        /// Github: https://github.com/johnspice/Calculus-C-dll-and-Java-Android-
        /// 
        /// </summary>
        /// <param name="expresion"></param>
        private static void fncOperacion(string expresion)
        {
            double fx, x;
            Calculo AnalizadorDeFunciones = new Calculo();

            if (AnalizadorDeFunciones.Sintaxis(expresion, 'x')) 
            {
                fx = AnalizadorDeFunciones.EvaluaFx(1);
                

                Console.WriteLine("Resultado: "+fx.ToString());

            }           

            
        }

        /// <summary>
        /// Este metodo utiliza una expresion regular para validar de que
        /// lo ingresado sea una expresion matematica devolviendo un true en caso
        /// de que efectivamente sea una expresion matematica, devuelve un false en caso
        /// contrario
        /// 
        /// Para ello, los unicos parametros que posee la expresion regular es que contenga 
        /// numeros, simbolos matematicos y parentesis
        /// </summary>
        /// <param name="strExpression"></param>
        /// <returns></returns>
        private static bool fncVerifyExpressionByRegex(string strExpression)
        {
            string strRegex = "^[0-9+\\-*/().\\s]*$";
            return Regex.IsMatch(strExpression, strRegex) ? true : false;
        }

        /// <summary>
        /// Metodo utilizado para comprobar que efectivamente
        /// los parentesis dentro de la expresion sean equivalentes
        /// Si se habre uno, que este cerrado        
        /// </summary>
        /// <param name="strCadena"></param>
        /// <returns></returns>
        private static bool fncVerificarParentesis(string strCadena)
        {
            
            Stack<char> stkPila = new Stack<char>();
            foreach (char caracter in strCadena)
            {
                
                if (fncIsOpeningCharacter(caracter))
                {
                    stkPila.Push(caracter);
                }
                else if (fncIsClosingCharacter(caracter))
                {
                    if (stkPila.Count == 0 || !fncIsPairOf(stkPila.Pop(), caracter) )
                    {
                        return false;
                    }
                }
            }

            return stkPila.Count == 0;
        }

        /// <summary>
        /// Este metodo Recibe un caracter con el hace la comparacion de si es
        /// un parentesis de apertura, devolviendo true en caso de que si lo sea
        /// y false en el caso contrario
        /// </summary>
        /// <param name="chrCaracter"></param>
        /// <returns>True en caso de ser un parentesis de apertura
        /// False en caso de no serlo</returns>
        private static bool fncIsOpeningCharacter(char chrCaracter)
        {
            return (chrCaracter == '(') ? true : false;
        }


        /// <summary>
        /// Este metodo Recibe un caracter con el hace la comparacion de si es
        /// un parentesis de cierre, devolviendo true en caso de que si lo sea
        /// y false en el caso contrario
        /// </summary>
        /// <param name="chrCaracter"></param>
        /// <returns>True en caso de ser un parentesis de cierre
        /// False en caso de no serlo</returns>
        private static bool fncIsClosingCharacter(char chrCaracter)
        {
            return chrCaracter == ')';
        }

        /// <summary>
        /// Verifica que los parentesis esten debidamente cerrados, y que cada uno tenga su 'pareja'
        /// </summary>
        /// <param name="chrOpeningCharacter"></param>
        /// <param name="chrClosingCharacter"></param>
        /// <returns></returns>
        private static bool fncIsPairOf(char chrOpeningCharacter, char chrClosingCharacter)
        {
            return (chrOpeningCharacter == '(' && chrClosingCharacter == ')');
        }

    }
}
