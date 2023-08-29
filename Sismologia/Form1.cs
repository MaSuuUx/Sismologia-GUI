using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
 -------------------------------------------------------------------------------------------
 ===> NOMBRE: HECTOR MARCELO MONGE CABALLERO
 ===> CARNET: MC23084
 ===> PROFESOR: ING. BLADIMIR DIAZ
 ===> GL: 18

ENUNCIADO: El Departamento de Sismología de la Escuela de Ingeniería Civil de la Facultad de
Ingeniería y Arquitectura de la Universidad de El Salvador, necesita una solución que le
permita registrar la magnitud de los sismos que acontecen diariamente. Es decir, cada vez
que haya un sismo, se debe de registrar su magnitud. Al final del día se desea imprimir la
cantidad de sismos acontecidos.

 -------------------------------------------------------------------------------------------
 */
namespace Sismologia
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            BtnIngresar.Enabled = false;//DESHABILITAR BOTON AL CARAGAR EL FORMS
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();//CERRAR FORM
        }

        //EVENTO PARA EL BtnIngresar
        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            string entradaData = textBox1.Text;

            if (EsNumeroDecimal(entradaData))
            {
                decimal numero = decimal.Parse(entradaData);

                GuardarEnArchivo(numero.ToString()); // GUARDAR ARCHIVOS
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            controlBtn();
        }

        private void controlBtn()
        {
            string entaradaData = textBox1.Text;

            if (!string.IsNullOrWhiteSpace(entaradaData) && EsNumeroDecimal(entaradaData))
            {
                BtnIngresar.Enabled = true; // HABILITA BOTON SI EL CAMPO ES VALIDO
                errorProvider.Clear(); // LIMPIA MSJ ERROR
            }
            else
            {
                BtnIngresar.Enabled = false; // DESHABILITA BOTON SI EL CAMPO NO ES VALIDO
                errorProvider.SetError(textBox1, "Debe ingresar numeros en la escala de Richter (decimales)"); //MSJ DE ERROR
            }
        }

        static bool EsNumeroDecimal(string input)
        {
            string patron = @"^[+-]?\d+(\.\d+)?$";
            return System.Text.RegularExpressions.Regex.IsMatch(input, patron);
        }

        private void GuardarEnArchivo(string contenido)
        {
            try
            {
                string archivo = "Datos.txt";
                string fechaActual = DateTime.Now.ToString(); // FECHA ACTUAL

                using (StreamWriter sw = File.AppendText(archivo))
                {
                    sw.WriteLine(fechaActual + " - " + contenido); // GUARDAR FECHA Y SU CONTENIDO
                }

                MessageBox.Show("Datos guardados con éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en el archivo: " + ex.Message);
            }
        }

        //METODO PARA IMPRIMIR LOS ARCHIVOS
        private void Imprimir()
        {
            try
            {
                string archivo = "Datos.txt";

                if (File.Exists(archivo))//VERIFICA SI EL ARCHIVO EXISTE
                {
                    string[] lineas = File.ReadAllLines(archivo);//LEE CADA LINEA DEL ARCHIVO Y GUARDA SUS DATOS
                    List<string> datosHoy = new List<string>(); //CREA UNA LISTA PARA GUARDAR LOS DATOS

                    foreach (var linea in lineas) //RECORRER CADA LINEA DEL ARCHIVO
                    {
                        string[] partes = linea.Split(new[] { " - " }, StringSplitOptions.None);

                        if (partes.Length == 2)
                        {
                            string fechaString = partes[0];//EXTRAER LA FECHA
                            if (DateTime.TryParse(fechaString, out DateTime fecha))//CONVIERTE STRING A DATETIME
                            {
                                if (fecha.Date == DateTime.Now.Date) // FILTRA Y COMPARA CON LA FECHA DEL PC
                                {
                                    datosHoy.Add(linea);
                                }
                            }
                        }
                    }

                    if (datosHoy.Count > 0)//VERIFICA SI HAY DATOS ENCONTRADOS
                    {
                        string datosParaImprimir = string.Join(Environment.NewLine, datosHoy);
                        MessageBox.Show("Datos ingresados hoy:\n" + datosParaImprimir, "Datos en el archivo");
                    }
                    else
                    {
                        MessageBox.Show("No hay datos ingresados hoy.", "Datos en el archivo");
                    }
                }
                else
                {
                    MessageBox.Show("El archivo no existe.", "Datos en el archivo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir los datos: " + ex.Message);
            }

        }

        //EVENTO DEL BtnPrint_Cilck
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Imprimir();
        }
    }
}



