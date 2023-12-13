using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Practica04
{
    public partial class frmClasificaClientes : Form
    {
        public frmClasificaClientes()
        {
            InitializeComponent();
        }

        private void frmClasificaClientes_Load(object sender, EventArgs e)
        {
            // -------------------------------------------------------
            // cambia el texto de la cabecera del formulario
            // -------------------------------------------------------

            // HMenu es la clase que es llamada para que traiga en la variable cia el nombre de la compañia
            this.Text = HMenu.cia + " Clasificacion de Clientes";
        }

        private void txtClaseCte_KeyPress(object sender, KeyPressEventArgs e)
        {
            // aqui pregunta si la tecla de funcion que fue presionada es ENTER
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;

                // esta preguntando si el contenido del textbox es diferente de vacio
                if (txtClaseCte.Text.Trim() != string.Empty)
                {
                    // aqui esta preguntado que si lo digitado es solo numero y si la tecla de funcion presionada es diferente la tecla BackSpace
                    if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
                    {
                        // aqui mueve el foco del cursor al siguiente textbox en este caso es txtNombrePosicion
                        txtNombreClaseCte.Focus();
                    }
                    else  // else significa... de lo contrario haz esto
                    {
                        MessageBox.Show("Solo números", "MSJ",  // abre la caja de mensajes y despliega el mensaje que dice Solo Números
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                        e.Handled = true;
                        return;
                    }
                }
            }

        }

        private void txtClaseCte_Leave(object sender, EventArgs e)
        {
            // esta preguntando si el contenido del textbox es diferente de vacio
            if (txtClaseCte.Text.Trim() != string.Empty)
            {

                txtNombreClaseCte.Text = Busco.ClaseCte(Convert.ToString(txtClaseCte.Text));

                txtSecuencia.Text = Busco.ClaseCteSec(Convert.ToString(txtClaseCte.Text));
            }
        }

        private void txtNombreClaseCte_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtNombreClaseCte.Text.Trim() != string.Empty)
                {
                    // mueve el cursor al textbox txtDepartamento
                    txtSecuencia.Focus();
                }
            }
        }

        private void txtSecuencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtSecuencia.Text.Trim() != string.Empty)
                {
                    // mueve el cursor al textbox txtDepartamento
                    btnGuardar.Focus();
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtClaseCte.Text.Trim() != string.Empty)
            {
                BorrarInformacion(Convert.ToString(txtClaseCte.Text));  // Borra la informacion en la tabla POSICIONES en la base de datos antes de grabar
                                                                     // y se envia Convert.ToString(txtPosicion.Text) como parametro que es recibido 
                                                                     // por la funcion BorrarInformacion.

                GuardarInformacion();                                   // Inserta la informacion en la tabla POSICIONES en la base de datos
                btnLimpiar.PerformClick();                              // invoca el boto limpiar (btnLimpiar) y ejecuta su contenido
                                                                        // ========================================================================
                                                                      // Actualiza Secuencia en la tabla Clasifica_Cte
                                                                                               // ========================================================================

            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

            LimpiarEsteFormulario();  // viaja hasta la funcion LimpiarEsteFormulario() y ejecuta su contenido

            txtClaseCte.Focus();      // mueve el cursor al textBox indicado

        }

        private void bntBorrar_Click(object sender, EventArgs e)
        {
            if (txtClaseCte.Text.Trim() != string.Empty)
            {
                // Borra la informacion en la tabla POSICIONES en la base de datos antes de grabar
                // y se envia Convert.ToString(txtPosicion.Text) como parametro que es recibido 
                // por la funcion BorrarInformacion.
                BorrarInformacion(Convert.ToString(txtClaseCte.Text));

                btnLimpiar.PerformClick();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra este formulario
        }

        private void btnConsultaClaseCte_Click(object sender, EventArgs e)
        {
            txtClaseCte.Focus();  // el curso se queda en el textbox txtdepartamento

            // creo el objeto frm y se le asigna el formulario frmVENDEPTO al objeto
            frmVENCLACLIENTE frm = new frmVENCLACLIENTE();

            DialogResult res = frm.ShowDialog();  // aqui muestra la ventana
                                                  // res = obtiene el valor de la ventana de consulta abierta
                                                  // obtendra el valor de ok cuando cierre la ventana

            if (res == DialogResult.OK)           // aqui pregunta que si res es igual a ok ejecute el siguiente codigo
            {
                txtClaseCte.Text = frm.varf1; // txtdepartamento se le asignara el valor contenido en la variable varf1
                txtNombreClaseCte.Text = frm.varf2; // txtdepartamento se le asignara el valor contenido en la variable varf2
                
            }
        }

       

        private void GuardarInformacion()
        {
            SqlConnection cnGuardar = new SqlConnection(cnn.db);
            cnGuardar.Open();   // aqui abre la base de datos

            // script sql para insertar data dentro de la tabla POSICIONES por medio de parametros de entrada
            SqlCommand miqueri = new SqlCommand("INSERT INTO Clasifica_Cte (Clasifica, Descripcion, SECUENCIA) VALUES (@A0, @A1, @A2);", cnGuardar);

            miqueri.Parameters.AddWithValue("@A0", Convert.ToString(txtClaseCte.Text));        // asignado valor al paramentro mediante los textbox
            miqueri.Parameters.AddWithValue("@A1", Convert.ToString(txtNombreClaseCte.Text));
            miqueri.Parameters.AddWithValue("@A2", Convert.ToString(txtSecuencia.Text));


            miqueri.ExecuteNonQuery(); // ejecuta el query
            cnGuardar.Close();         // cierra la conexion a la base de datos

        }

        private void BorrarInformacion(string numClaseCte)
        {
            // ----------------------------------------------------------
            // se crea la conexion hacia SQL
            // cnn.db ... contiene el string conexion a la base de datos
            // cnndel ... es el objeto creado que manipulara la conexion a la base de datos
            // ----------------------------------------------------------
            SqlConnection cnndel = new SqlConnection(cnn.db);
            cnndel.Open();  // aqui abre la base de datos

            // aqui se arma el script de sql y contiene un parametro @RG
            // string miQuery = "DELETE FROM POSICIONES WHERE IDPOSICION = @RG;";

            SqlCommand queri = new SqlCommand("DELETE FROM Clasifica_Cte WHERE Clasifica = @RG;", cnndel);  // crea el comando para ejecutar el script de sql
            queri.Parameters.AddWithValue("@RG", numClaseCte);   // aqui se le envia la data contenida en numPosicion al parametro @RG
            queri.ExecuteNonQuery();                             // ejecuta el query

            cnndel.Close();                                      // cierra la conexion a la base de datos
          
        }

        private void LimpiarEsteFormulario()
        {
            // -----------------------------
            // Limpia los textBox and Label
            // -----------------------------
            txtClaseCte.Clear();
            txtNombreClaseCte.Clear();
            txtSecuencia.Clear();

        }

       
    }



}
