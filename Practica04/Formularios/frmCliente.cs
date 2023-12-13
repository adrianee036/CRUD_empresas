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
    public partial class frmCliente : Form
    {
        Boolean ExisteElCliente;

        public frmCliente()
        {
            InitializeComponent();
        }

        private void frmCliente_Load(object sender, EventArgs e)
        {
            this.Text = HMenu.cia + " Maestro de Cliente " + HMenu.pc;
            this.KeyPreview = true;    // activa la teclas de funciones y presionar la tecla escape en el form

            ExisteElCliente = false;
        }

        private void frmCliente_KeyDown(object sender, KeyEventArgs e) // Se declara la clase como privada con el nombre frmCliente_KeyDown
        {
            if (e.KeyCode == Keys.Escape) // Si la      // ==============================================================
            {                                           //
                                                        // Si se presiona la tecla Escape se cierra el programa
                                                        //
                this.Close();                           // ==============================================================
            }
        }

        private void txtClase_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtClase.Text.Trim() != string.Empty)
                {
                    txtCliente.Focus();  // mueve el cursor al textbos txtCliente
                }
            }
        }

        private void txtClase_Leave(object sender, EventArgs e)
        {
            if (txtClase.Text.Trim() != string.Empty)
            {
                txtClase.Text = Convert.ToInt32(txtClase.Text).ToString("000"); 

                SecuenciaDelCliente(Convert.ToString(txtClase.Text));
            }
        }

        private void txtCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtClase.Text.Trim() != string.Empty)
                {
                    txtNombre.Focus();
                }
            }
        }

        private void txtCliente_Leave(object sender, EventArgs e) // Se declara la clase como privada con el nombre txtCliente_Leave
        {
            if (txtClase.Text.Trim() != string.Empty) // Si el textbox txtClase esta vacio no se ejecuta el codigo debajo
            {
                txtCliente.Text = Convert.ToInt32(txtCliente.Text).ToString("00000000"); // Formatea el textbox txtCliente con 8 digitos numericos

                BuscarCliente(); // Ejecuta el método BuscarCliente();
            }
        }

        // ===============================================================================================
        // Botones
        // ===============================================================================================
        private void btnClasifica_Click(object sender, EventArgs e)
        {
            txtClase.Focus();

            frmVENCLACLIENTE frm = new frmVENCLACLIENTE();

            DialogResult res = frm.ShowDialog();

            if (res == DialogResult.OK)
            {
                txtClase.Text = frm.varf1;
                lblDescripcion.Text = frm.varf2;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtClase.Text.Trim() != string.Empty && txtCliente.Text.Trim() != string.Empty)
            {
                // ========================================================================
                // coloca aqui guardar en la tabla cliente
                // ========================================================================
                if (ExisteElCliente == true)
                {
                    BorrarInformacion(txtCliente.Text);
                }

                GuardarInformacion();

                // ========================================================================
                // Actualiza Secuencia en la tabla Clasifica_Cte
                // ========================================================================
                if (ExisteElCliente == false)
                {
                    SqlConnection cnUpdate = new SqlConnection(cnn.db);
                    cnUpdate.Open();

                    SqlCommand miUpdate = new SqlCommand("UPDATE Clasifica_Cte  SET Secuencia = @A1  FROM Clasifica_Cte  WHERE Clasifica =@A0", cnUpdate);

                    miUpdate.Parameters.AddWithValue("@A0", Convert.ToString(txtClase.Text));
                    miUpdate.Parameters.AddWithValue("@A1", Convert.ToString(txtCliente.Text));

                    miUpdate.ExecuteNonQuery();
                    cnUpdate.Close();
                }

                LimpiarFormulario();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void bntBorrar_Click(object sender, EventArgs e)
        {
            if (txtClase.Text.Trim() != string.Empty && txtCliente.Text.Trim() != string.Empty)
            {
                if (ExisteElCliente == true)
                {
                    BorrarInformacion(txtCliente.Text);
                    LimpiarFormulario();
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConsultaCliente_Click(object sender, EventArgs e)
        {
            frmConsultaCliente frm = new frmConsultaCliente();

            DialogResult res = frm.ShowDialog();

            //if (res == DialogResult.OK)
            //{
                txtClase.Text = frm.varf1;
                txtCliente.Text = frm.varf2;
                txtNombre.Text = frm.varf3;

                BuscarCliente();
            //}
        }

        // ===============================================================================================
        // Metodos
        // ===============================================================================================
        private void LimpiarFormulario()
        {
            ExisteElCliente = false;

            txtClase.Clear();
            txtCliente.Clear();
            txtNombre.Clear();
            txtDireccion.Clear();
            txtSector.Clear();
            txtCiudad.Clear();
            txtEdificio.Clear();
            txtTelCasa.Clear();
            txtTelMovil.Clear();
            txtContacto.Clear();
            txtCorreo.Clear();
            lblDescripcion.Text = "";

            txtClase.Focus();
        }

        private void BorrarInformacion(string numeroCliente)
        {
            SqlConnection cnndel = new SqlConnection(cnn.db);
            cnndel.Open();  

            SqlCommand queri = new SqlCommand("DELETE FROM CLIENTES WHERE CLIENTE = @RG;", cnndel);

            queri.Parameters.AddWithValue("@RG", numeroCliente);   
            queri.ExecuteNonQuery();                                 

            cnndel.Close();                                          
        }

        private void GuardarInformacion()
        {
            SqlConnection cnGuardar = new SqlConnection(cnn.db);
            cnGuardar.Open();   

            SqlCommand miQueri = new SqlCommand("INSERT INTO CLIENTES (CLIENTE, CLASIFICACTE, NOMBRE, DIRECCION, SECTOR, CIUDAD, " +
                                                "                      EDIFICIO, TEL_CASA, TEL_MOVIL, EMAIL, nombredelCONTACTO ) " +
                                                "VALUES (@A0, @A1, @A2,  @A3, @A4, @A5, @A6, @A7, @A8, @A9, @A10);", cnGuardar);

            miQueri.Parameters.AddWithValue("@A0", Convert.ToString(txtCliente.Text)); 
            miQueri.Parameters.AddWithValue("@A1", Convert.ToString(txtClase.Text));  
            miQueri.Parameters.AddWithValue("@A2", Convert.ToString(txtNombre.Text));
            miQueri.Parameters.AddWithValue("@A3", Convert.ToString(txtDireccion.Text));
            miQueri.Parameters.AddWithValue("@A4", Convert.ToString(txtSector.Text));
            miQueri.Parameters.AddWithValue("@A5", Convert.ToString(txtEdificio.Text));
            miQueri.Parameters.AddWithValue("@A6", Convert.ToString(txtCiudad.Text));
            miQueri.Parameters.AddWithValue("@A7", Convert.ToString(txtTelCasa.Text));
            miQueri.Parameters.AddWithValue("@A8", Convert.ToString(txtTelMovil.Text));
            miQueri.Parameters.AddWithValue("@A9", Convert.ToString(txtCorreo.Text));
            miQueri.Parameters.AddWithValue("@A10", Convert.ToString(txtContacto.Text));

            miQueri.ExecuteNonQuery(); 
            cnGuardar.Close();         
        }

        private void BuscarCliente()
        {
            if (txtCliente.Text.Trim() != string.Empty && txtClase.Text.Trim() != string.Empty)
            {
                SqlConnection cnxn = new SqlConnection(cnn.db);
                cnxn.Open();

                SqlCommand cmmnd = new SqlCommand("     SELECT A.ClasificaCte,	    A.Cliente,	A.Nombre,	A.Direccion, A.Sector, " +
                                                  "            A.Ciudad,    	    A.Edificio,	A.Tel_Casa,	A.Tel_Movil, A.Email, " +
                                                  "            A.NombreDelContacto, B.DESCRIPCION " +
                                                  "       FROM CLIENTES  A  " + 
                                                  " INNER JOIN CLASIFICA_CTE B  ON  A.CLASIFICACTE = B.CLASIFICA " +
                                                  "      WHERE CLIENTE = @PV", cnxn);

                cmmnd.Parameters.AddWithValue("@PV", txtCliente.Text);

                SqlDataReader recordn = cmmnd.ExecuteReader();

                ExisteElCliente = false;

                if (recordn.Read())
                {
                    ExisteElCliente = true;

                    lblDescripcion.Text = Convert.ToString(recordn["Descripcion"]);
                    txtNombre.Text = Convert.ToString(recordn["Nombre"]);
                    txtDireccion.Text = Convert.ToString(recordn["Direccion"]);
                    txtSector.Text = Convert.ToString(recordn["Sector"]);
                    txtCiudad.Text = Convert.ToString(recordn["Ciudad"]);
                    txtEdificio.Text = Convert.ToString(recordn["Edificio"]);
                    txtTelCasa.Text = Convert.ToString(recordn["Tel_Casa"]);
                    txtTelMovil.Text = Convert.ToString(recordn["Tel_Movil"]);
                    txtContacto.Text = Convert.ToString(recordn["NombreDelContacto"]);
                    txtCorreo.Text = Convert.ToString(recordn["Email"]);
                }
            }
        }

        private void SecuenciaDelCliente(string numeroClasificacion)
        {
            SqlConnection cnxn = new SqlConnection(cnn.db);
            cnxn.Open();

            SqlCommand cmmnd = new SqlCommand("SELECT DESCRIPCION, SECUENCIA  FROM CLASIFICA_CTE  WHERE CLASIFICA = @PV", cnxn);
            cmmnd.Parameters.AddWithValue("@PV", numeroClasificacion);
            SqlDataReader rdr = cmmnd.ExecuteReader();

            if (rdr.Read())
            {
                int secuencia = Convert.ToInt32(rdr["SECUENCIA"]);
                secuencia = secuencia + 1;

                txtCliente.Text = secuencia.ToString("00000000");
                lblDescripcion.Text = Convert.ToString(rdr["DESCRIPCION"]);
            }

            cmmnd.Dispose();
            cnxn.Close();
        }
    }
}
