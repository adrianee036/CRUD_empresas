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
    public partial class frmVENPOS : Form
    {
        public string varf1;  // variable publica usada para transporta la data asignada en ella hacia el formulario padre
        public string varf2;
        public string varf3;
        public string varf4;
        public string varf5;
        public string varf6;

        public frmVENPOS()
        {
            InitializeComponent();
        }

        private void frmVENPOS_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;     // puedo presionar la tecla escape
            this.Text = HMenu.cia + "   Consulta Posicion";

            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.ColumnHeadersVisible = true;
            this.dgv.RowHeadersVisible = false;

            this.dgv.Columns.Add("Col00", "ID de Posicion");
            this.dgv.Columns.Add("Col01", "Nombre de Posicion");
            this.dgv.Columns.Add("Col02", "ID Fabrica");
            this.dgv.Columns.Add("Col03", "ID Departamento");
            this.dgv.Columns.Add("Col04", "Nombre de la Fabrica");
            this.dgv.Columns.Add("Col05", "Nombre del Departamento");

            DataGridViewColumn
            column = dgv.Columns[0]; column.Width = 100;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[1]; column.Width = 100;  // le esta indicando el ancho de la columna en pixel
            column = dgv.Columns[2]; column.Width = 100;
            column = dgv.Columns[3]; column.Width = 100;
            column = dgv.Columns[4]; column.Width = 100;
            column = dgv.Columns[5]; column.Width = 100;

            Estilodgv();  // hace un llamado a esta funcion
        }

        private void Estilodgv()
        {
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

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtBuscar.Text.Trim() != string.Empty)
                {
                    btnBuscar.PerformClick(); // ejecuta el codigo contenido en el evento Click del boton btnSeleccion
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarDatos();    // hace un llamado a esta funcion
        }

        private void BuscarDatos()
        {
            // dgv es el nombre de la grilla
            this.dgv.Rows.Clear();  // Limpia la grilla
            this.dgv.Refresh();     // actualiza la grilla

            SqlConnection conexion = new SqlConnection(cnn.db);
            conexion.Open();   // abre la conexion a la Base de Datos
            SqlCommand sqCmd = new SqlCommand("    SELECT A.IDposicion,  " +
                                              "           A.NombreDePosicion,  " +
                                              "           A.Fabrica,  " +
                                              "           A.Departamento,  " +
                                              "           B.NombreDefabrica,   " +
                                              "           C.NombreDepartamento  " +
                                              "      FROM POSICIONES   A " +
                                              " LEFT JOIN FABRICA      B ON A.Fabrica      = B.IDfabrica " +
                                              " LEFT JOIN DEPARTAMENTO C ON A.Departamento = C.IDdepartamento " +
                                              "   WHERE NombreDePosicion LIKE '%" + txtBuscar.Text + 
                                              "%' ORDER BY NombreDePosicion ASC", conexion);
            SqlDataReader recordset = sqCmd.ExecuteReader();

            try
            {
                while (recordset.Read())
                {  // no es fin de archivo true false
                    dgv.Rows.Add();                                                       // aqui le suma 1 a la fila, es decir x = x + 1 
                    int xRows = dgv.Rows.Count - 1;                                       // aqui le resto 1 para me indique la fila correcta en donde estoy
                    dgv[0, xRows].Value = Convert.ToString(recordset["IDposicion"]);       // escribe en la grilla, en la celda 0 y en la fila (x), el contenido del campo IDDEPARTAMENTO     de la tabla DEPARTAMENTO
                    dgv[1, xRows].Value = Convert.ToString(recordset["NombreDePosicion"]); // escribe en la grilla, en la celda 1 y en la fila (x), el contenido del campo NOMBREDEPARTAMENTO de la tabla DEPARTAMENTO
                    dgv[2, xRows].Value = Convert.ToString(recordset["Fabrica"]);
                    dgv[3, xRows].Value = Convert.ToString(recordset["Departamento"]);
                    dgv[4, xRows].Value = Convert.ToString(recordset["NombreDefabrica"]);
                    dgv[5, xRows].Value = Convert.ToString(recordset["NombreDepartamento"]);
                }
            }
            catch
            {
                //
            }

            sqCmd.Dispose();    // libera el espacio de memoria ocupado con la data recopilada
            conexion.Close();   // cierra la conexion a la base de datos
        }

        private void btnSeleccion_Click(object sender, EventArgs e)
        {
            try
            {   // dgv significa Data grid View 
                varf1 = dgv.CurrentRow.Cells[0].Value.ToString();  // selecciona el valor contenido en la celda actual de la columna 0 de la fila seleccionada
                varf2 = dgv.CurrentRow.Cells[1].Value.ToString();  // selecciona el valor contenido en la celda actual de la columna 1 de la fila seleccionada
                varf3 = dgv.CurrentRow.Cells[2].Value.ToString();
                varf4 = dgv.CurrentRow.Cells[3].Value.ToString();
                varf5 = dgv.CurrentRow.Cells[4].Value.ToString();
                varf6 = dgv.CurrentRow.Cells[5].Value.ToString();

                this.Close();  // cierra la ventana
            }
            catch
            {
                //
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close(); // cierra la ventana
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnSeleccion.PerformClick();  // ejecuta el codigo contenido en el evento Click del boton btnSeleccion
        }
    }
}
