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
    public partial class frmConsultaCliente : Form
    {
        public string varf1;  // variable publica usada para transporta la data asignada en ella hacia el formulario padre
        public string varf2;  // variable publica usada para transporta la data asignada en ella hacia el formulario padre
        public string varf3;

        public frmConsultaCliente()
        {
            InitializeComponent();
            Estilodgv();
        }

        private void frmConsultaCliente_Load(object sender, EventArgs e)
        {
            this.Text = HMenu.cia + " Consulta Clientes " + HMenu.pc;
            this.KeyPreview = true;    // activa la teclas de funciones y presionar la tecla escape en el form
        }

        private void frmConsultaCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtBuscar.Text.Trim() != string.Empty)
                {
                    btnBuscar.Focus();
                }
            }
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            btnBuscar.Focus();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarDatos();
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            try
            {   // dgv significa Data grid View 
                varf1 = dgv.CurrentRow.Cells[0].Value.ToString();  // selecciona el valor contenido en la celda actual de la columna 0 de la fila seleccionada
                varf2 = dgv.CurrentRow.Cells[1].Value.ToString();  // selecciona el valor contenido en la celda actual de la columna 1 de la fila seleccionada
                varf3 = dgv.CurrentRow.Cells[2].Value.ToString();  // selecciona el valor contenido en la celda actual de la columna 1 de la fila seleccionada

                this.Close();  // cierra la ventana
            }
            catch
            {
                //
            }
        }

        private void Estilodgv()
        {
            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.AllowUserToAddRows = false;    // no permite que se agregue una linea en blanco
            this.dgv.AllowUserToDeleteRows = false; // Permitir al usuario eliminar filas 
            this.dgv.ColumnHeadersVisible = true;   // Encabezados de columna visibles
            this.dgv.RowHeadersVisible = false;     // Encabezados de fila visibles

            this.dgv.Columns.Add("Col00", "Clasifica");
            this.dgv.Columns.Add("Col01", "Cliente");
            this.dgv.Columns.Add("Col02", "Nombre");
            this.dgv.Columns.Add("Col03", "Direccion");
            this.dgv.Columns.Add("Col04", "Sector");
            this.dgv.Columns.Add("Col05", "Edificio");
            this.dgv.Columns.Add("Col06", "Ciudad");
            this.dgv.Columns.Add("Col07", "Tel Casa");
            this.dgv.Columns.Add("Col08", "Tel Movil");
            this.dgv.Columns.Add("Col09", "Correo");
            this.dgv.Columns.Add("Col10", "Contacto");

            DataGridViewColumn
            column = dgv.Columns[0]; column.Width = 150;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[1]; column.Width = 300;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[2]; column.Width = 200;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[3]; column.Width = 200;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[4]; column.Width = 200;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[5]; column.Width = 200;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[6]; column.Width = 200;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[7]; column.Width = 200;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[8]; column.Width = 200;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[9]; column.Width = 200;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[10]; column.Width = 200;  // le esta indicando el ancho de la columna en pixel

            // dgv es el nombre de la grilla
            this.dgv.BorderStyle = BorderStyle.None;                                 // el data grid view no tendra border en los lados
            this.dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.Aqua;         //Color.FromArgb(238, 239, 249);
            this.dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // le indica que tendra una linea simple horizontal entre las filas
            this.dgv.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;      // le cambia el color al background de la linea cuando esta es seleccionada
            this.dgv.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;         // le cambia el color a las letras de la linea cuando esta es seleccionada
            this.dgv.BackgroundColor = Color.LightGray;                              // cambia el color de fondo del data grid view

            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;    // el header no tendra borde
            this.dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 6, 0, 6);  // le indica el ancho de la fila de la cabecera
            this.dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.CornflowerBlue;   // el color que tendra la cabecera o header
            this.dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;            // el color de la letras en la cabecera o header
        }

        private void BuscarDatos()
        {
            // dgv es el nombre de la grilla
            this.dgv.Rows.Clear();  // Limpia la grilla
            this.dgv.Refresh();     // actualiza la grilla

            SqlConnection conexion = new SqlConnection(cnn.db);
            conexion.Open();   // abre la conexion a la Base de Datos
            SqlCommand sqCmd = new SqlCommand("SELECT CLIENTE, CLASIFICACTE, NOMBRE, DIRECCION, SECTOR, CIUDAD, " 
                                            + "                      EDIFICIO, TEL_CASA, TEL_MOVIL, EMAIL, nombredelCONTACTO  " 
                                            + "  FROM CLIENTES "
                                            + " WHERE NOMBRE LIKE '%" + txtBuscar.Text + "%'"
                                            + " ORDER BY NOMBRE ASC", conexion);
            SqlDataReader recordset = sqCmd.ExecuteReader();

            try
            {
                while (recordset.Read())
                {  // no es fin de archivo true false
                    dgv.Rows.Add();                                                       // aqui le suma 1 a la fila, es decir x = x + 1 
                    int xRows = dgv.Rows.Count - 1;                                       // aqui le resto 1 para me indique la fila correcta en donde estoy
                    dgv[0, xRows].Value = Convert.ToString(recordset["ClasificaCte"]);       // escribe en la grilla, en la celda 0 y en la fila (x), el contenido del campo IDDEPARTAMENTO     de la tabla DEPARTAMENTO
                    dgv[1, xRows].Value = Convert.ToString(recordset["Cliente"]);     // escribe en la grilla, en la celda 1 y en la fila (x), el contenido del campo NOMBREDEPARTAMENTO de la tabla DEPARTAMENTO
                    dgv[2, xRows].Value = Convert.ToString(recordset["Nombre"]);
                    dgv[3, xRows].Value = Convert.ToString(recordset["DIRECCION"]);
                    dgv[4, xRows].Value = Convert.ToString(recordset["SECTOR"]);
                    dgv[5, xRows].Value = Convert.ToString(recordset["CIUDAD"]);
                    dgv[6, xRows].Value = Convert.ToString(recordset["EDIFICIO"]);
                    dgv[7, xRows].Value = Convert.ToString(recordset["TEL_CASA"]);
                    dgv[8, xRows].Value = Convert.ToString(recordset["TEL_MOVIL"]);
                    dgv[9, xRows].Value = Convert.ToString(recordset["EMAIL"]);
                    dgv[10, xRows].Value = Convert.ToString(recordset["nombredelCONTACTO"]);
                }
            }
            catch
            {
                //
            }

            sqCmd.Dispose();    // libera el espacio de memoria ocupado con la data recopilada
            conexion.Close();   // cierra la conexion a la base de datos
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnSeleccionar.PerformClick();
        }
    }
}
