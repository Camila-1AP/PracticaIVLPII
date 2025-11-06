using CrudORMentity.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CrudORMentity
{
    public partial class FrmCategorias : Form
    {
        private Actividad1Entities _context;
        public FrmCategorias()
        {
            InitializeComponent();
        }

        private void cargarDatos()
        {
            var listaCategorias = _context.Categorias
                   .Select(p => new {
                       ID = p.CategoriaID,
                       Nombre_Categoría = p.NombreCategoria,
                       
                   }).ToList();

            dgv.DataSource = listaCategorias;
        }

        

        private void Categorias_Load(object sender, EventArgs e)
        {
            _context = new Actividad1Entities();

      
            this.cargarDatos();


        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("El campo de nombre esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Categorias categoria = new Categorias()
            {
                NombreCategoria = txtNombre.Text

            };

            _context.Categorias.Add(categoria);

            int rowsAffected = _context.SaveChanges();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Se ha insertado la categoría en la base de datos.");
            }

            this.cargarDatos();
            txtNombre.Clear();
        }

        private void Delete_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Debe introducir un ID válido.");
                return;
            }
            if (MessageBox.Show("Recuerde al aceptar eliminar esta categoría, los productos, detalles factura y detalles compra, relacionados con los produtos en dicha categoría se eliminarán. Quizás quiera revisarlos antes.", "Advertencia de Eliminación de categoría", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                int categoriaID = Convert.ToInt32(txtID.Text);

                Categorias categoria = _context.Categorias.FirstOrDefault(q => q.CategoriaID.Equals(categoriaID));
                if (categoria == null)
                {
                    MessageBox.Show("La Categoría no existe.");
                    return;
                }
                //detallesFactura y detallesCompra

                _context.DetallesFactura.RemoveRange(categoria.Productos.SelectMany(p => p.DetallesFactura));
                _context.DetallesCompra.RemoveRange(categoria.Productos.SelectMany(p => p.DetallesCompra));
                _context.Productos.RemoveRange(categoria.Productos);
                _context.Categorias.Remove(categoria);
             
                int rowsAffected = _context.SaveChanges();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Se ha eliminado la categoría en la base de datos.");
                }

                this.cargarDatos();
                txtID.Clear();


            }

        }

        private void Update_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtIDUp.Text))
            {
                MessageBox.Show("El campo de ID esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(txtNombreUp.Text))
            {
                MessageBox.Show("El campo de nombre esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            int categoriaID = Convert.ToInt32(txtIDUp.Text);

            Categorias categoria = _context.Categorias.FirstOrDefault(q => q.CategoriaID.Equals(categoriaID));
            if (categoria == null)
            {
                MessageBox.Show("La Categoria no existe.");
                return;
            }

            categoria.NombreCategoria = txtNombreUp.Text;
            
            int rowsAffected = _context.SaveChanges();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Se ha actualizado la categoría en la base de datos.");
            }

            this.cargarDatos();
            txtIDUp.Clear();
            txtNombreUp.Clear();
        }

        private void View_Click(object sender, EventArgs e)
        {
            cargarDatos();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtNombreUp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtIDUp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
