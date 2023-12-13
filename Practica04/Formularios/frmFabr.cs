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
    public partial class frmFabr : Form
    {
        public frmFabr()
        {
            InitializeComponent();
        }

        private void frmFabr_Load(object sender, EventArgs e)
        {
            // -------------------------------------------------------
            // cambia el texto de la cabecera del formulario
            // -------------------------------------------------------

            // HMenu es la clase que es llamada para que traiga en la variable cia el nombre de la compañia
            this.Text = HMenu.cia + " Maestro de Fabrica";
        }

        private void txtFabrica_KeyPress(object sender, KeyPressEventArgs e)
        {
            // aqui pregunta si la tecla de funcion que fue presionada es ENTER
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;

                // esta preguntando si el contenido del textbox es diferente de vacio
                if (txtFabrica.Text.Trim() != string.Empty)
                {
                    // aqui esta preguntado que si lo digitado es solo numero y si la tecla de funcion presionada es diferente la tecla BackSpace
                    if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
                    {
                        // aqui mueve el foco del cursor al siguiente textbox en este caso es txtNombrePosicion
                        txtNombreFabrica.Focus();
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

        private void txtFabrica_Leave(object sender, EventArgs e)
        {
            // esta preguntando si el contenido del textbox es diferente de vacio
            if (txtFabrica.Text.Trim() != string.Empty)
            {
                // ----------------------------------------------------------------------
                // Busco.CargosPosiciones
                //
                // Busco...                              es la clase que se esta invocando
                // CargosPosiciones...                   es el metodo dento de la clase
                // Convert.ToString(txtPosicion.Text)... es el parametro enviado al metodo de la clase, convertido en un string
                // ----------------------------------------------------------------------
                txtNombreFabrica.Text = Busco.Fabrica(Convert.ToString(txtFabrica.Text));

                //txtNombrePosicion.Text = NombreCargo;
                buscar_datos();
            }
        }

        private void txtNombreFabrica_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtNombreFabrica.Text.Trim() != string.Empty)
                {
                    // mueve el cursor al textbox txtDepartamento
                    txtLocalidad.Focus();
                }
            }
        }

        private void txtNombreFabrica_Leave(object sender, EventArgs e)
        {
            txtLocalidad.Focus();
        }

        private void txtLocalidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtLocalidad.Text.Trim() != string.Empty)
                {
                    // mueve el cursor al textbox txtFabrica
                    btnGuardar.Focus();
                }
            }
        }

        private void txtLocalidad_Leave(object sender, EventArgs e)
        {
            if (txtLocalidad.Text.Trim() != string.Empty)
            {
                // ----------------------------------------------------------------------
                // Busco.NombreDepartamento
                // Busco...                                  es la clase que se esta invocando
                // NombreDepartamento...                     es el metodo dento de la clase
                // Convert.ToString(txtDepartamento.Text)... es el parametro enviado al metodo de la clase, convertido en un string
                // ----------------------------------------------------------------------

                lblFabricaNombre.Text = Busco.Localidad(Convert.ToString(txtLocalidad.Text)); // valor retornado por la clase es asignado al label lblDepartamento
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            BorrarInformacion(Convert.ToString(txtFabrica.Text));  // Borra la informacion en la tabla POSICIONES en la base de datos antes de grabar
            // y se envia Convert.ToString(txtPosicion.Text) como parametro que es recibido 
            // por la funcion BorrarInformacion.

            GuardarInformacion();                                   // Inserta la informacion en la tabla POSICIONES en la base de datos
            btnLimpiar.PerformClick();                              // invoca el boto limpiar (btnLimpiar) y ejecuta su contenido
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarEsteFormulario();  // viaja hasta la funcion LimpiarEsteFormulario() y ejecuta su contenido
            txtFabrica.Focus();      // mueve el cursor al textBox indicado
        }


        private void bntBorrar_Click(object sender, EventArgs e)
        {
            // Borra la informacion en la tabla POSICIONES en la base de datos antes de grabar
            // y se envia Convert.ToString(txtPosicion.Text) como parametro que es recibido 
            // por la funcion BorrarInformacion.
            BorrarInformacion(Convert.ToString(txtFabrica.Text));
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra este formulario
        }

        private void BorrarInformacion(string numPosicion)
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

            SqlCommand queri = new SqlCommand("DELETE FROM FABRICA WHERE IDfabrica = @RG;", cnndel);  // crea el comando para ejecutar el script de sql
            queri.Parameters.AddWithValue("@RG", numPosicion);   // aqui se le envia la data contenida en numPosicion al parametro @RG
            queri.ExecuteNonQuery();                             // ejecuta el query

            cnndel.Close();                                      // cierra la conexion a la base de datos
        }

        private void GuardarInformacion()
        {
            SqlConnection cnGuardar = new SqlConnection(cnn.db);
            cnGuardar.Open();   // aqui abre la base de datos

            // script sql para insertar data dentro de la tabla POSICIONES por medio de parametros de entrada
            string stQuery = "INSERT INTO FABRICA (IDfabrica, NombreDefabrica, Localidad) VALUES (@A0, @A1, @A2 );";
            SqlCommand miqueri = new SqlCommand(stQuery, cnGuardar); // crea el comando para ejecutar el script de sql

            miqueri.Parameters.AddWithValue("@A0", Convert.ToString(txtFabrica.Text));        // asignado valor al paramentro mediante los textbox
            miqueri.Parameters.AddWithValue("@A1", Convert.ToString(txtNombreFabrica.Text));
            miqueri.Parameters.AddWithValue("@A2", Convert.ToString(txtLocalidad.Text));

            miqueri.ExecuteNonQuery(); // ejecuta el query
            cnGuardar.Close();         // cierra la conexion a la base de datos
        }

        private void LimpiarEsteFormulario()
        {
            // -----------------------------
            // Limpia los textBox and Label
            // -----------------------------
            txtFabrica.Clear();
            txtNombreFabrica.Clear();
            txtLocalidad.Clear();
            lblFabricaNombre.Text = "";
        }

        private void buscar_datos()
        {
            SqlConnection cnxn = new SqlConnection(cnn.db);
            cnxn.Open();

            SqlCommand cmmnd = new SqlCommand("    SELECT a.NombreDefabrica,  " +
                                              "           a.Localidad,  " +
                                              "           B.NombreLocalidad  " +
                                              "      FROM FABRICA   A " +
                                              " LEFT JOIN LOCALIDAD      B ON A.Localidad      = B.IDLocalidad " +
                                              "WHERE IDfabrica =@PV", cnxn);

            cmmnd.Parameters.AddWithValue("@PV", txtFabrica.Text);

            SqlDataReader recordn = cmmnd.ExecuteReader();

            if (recordn.Read())
            {
                // los campos que contienen la data extraida de la tabla es asignado a cada elemento correspondiente
                txtNombreFabrica.Text = Convert.ToString(recordn["NOMBREDEFABRICA"]);
                txtLocalidad.Text = Convert.ToString(recordn["LOCALIDAD"]);
                lblFabricaNombre.Text = Convert.ToString(recordn["NOMBRELOCALIDAD"]);
            }

            cmmnd.Dispose();  // libera el sqlcommand 
            cnxn.Close();   // cierra la conexion a la base de datos
        }

        private void btnConsultaFabrica_Click(object sender, EventArgs e)
        {
            txtFabrica.Focus();  // el curso se queda en el textbox txtdepartamento

            // creo el objeto frm y se le asigna el formulario frmVENDEPTO al objeto
            frmVENFABR frm = new frmVENFABR();

            DialogResult res = frm.ShowDialog();  // aqui muestra la ventana
            // res = obtiene el valor de la ventana de consulta abierta
            // obtendra el valor de ok cuando cierre la ventana

            if (res == DialogResult.OK)           // aqui pregunta que si res es igual a ok ejecute el siguiente codigo
            {
                txtFabrica.Text = frm.varf1;       // txtdepartamento se le asignara el valor contenido en la variable varf1
                lblFabricaNombre.Text = frm.varf2; // txtdepartamento se le asignara el valor contenido en la variable varf2
            }
        }

    }

}
