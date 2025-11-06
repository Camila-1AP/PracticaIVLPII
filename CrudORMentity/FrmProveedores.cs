using CrudORMentity.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudORMentity
{
    public partial class FrmProveedores : Form
    {
       
        private Actividad1Entities _context;
        
        public FrmProveedores()
        {
            InitializeComponent();
        }
        private void cargaDatos()
        {
            var listaProveedores = _context.Proveedores

                .Select(p => new
                {
                    ID = p.ProveedorID,
                    Nombre_Proveedor = p.NombreProveedor,
                    Teléfono_Proveedor = p.Telefono,
                    CorreoElectrónico_Proveedor = p.CorreoElectronico,
                }).ToList();
            dgv.DataSource = listaProveedores;
        }
       
        private void Add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("El campo de nombre esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(maskTel.Text))
            {
                MessageBox.Show("El campo de teléfono esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!maskTel.MaskFull)
            {
                MessageBox.Show("Por favor ingresa un número de teléfono completo.");
                return;
             
            }
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("El campo de correo electrónico esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Regex.IsMatch(txtEmail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Correo electrónico inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
               
            }
            Proveedores proveedores = new Proveedores()
            {
                NombreProveedor = txtNombre.Text,
                Telefono = maskTel.Text,
                CorreoElectronico = txtEmail.Text,
            };
            _context.Proveedores.Add(proveedores);

            int rowsAffeted = _context.SaveChanges();
            if (rowsAffeted > 0)
            {
                MessageBox.Show("Se ha insertado el proveedor en la base de datos.");
            }
            this.cargaDatos();
            txtNombre.Clear();
            maskTel.Clear();
            txtEmail.Clear();

        }

        private void FrmProveedores_Load(object sender, EventArgs e)
        {
            _context = new Actividad1Entities();

            this.cargaDatos();

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Debe introducir un ID válido.");
                return;
            }
            int proveedorId = Convert.ToInt32(txtID.Text);
            Proveedores proveedores = _context.Proveedores.FirstOrDefault(q => q.ProveedorID.Equals(proveedorId));
            if (proveedores == null)
            {
                MessageBox.Show("El Proveedor no existe.");
            }
            foreach (var compra in proveedores.Compras.ToList())
            {
                _context.DetallesCompra.RemoveRange(compra.DetallesCompra);
            }
            _context.Compras.RemoveRange(proveedores.Compras);
            _context.Proveedores.Remove(proveedores);

            int rowsAffected = _context.SaveChanges();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Se ha eliminado el proveedor en la base de datos.");
            }
            this.cargaDatos();
            txtID.Clear();

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

            if (string.IsNullOrEmpty(maskTelUp.Text))
            {
                MessageBox.Show("El campo de teléfono esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!maskTelUp.MaskFull)
            {
                MessageBox.Show("Por favor ingresa un número de teléfono completo.");
                maskTelUp.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtEmailUp.Text))
            {
                MessageBox.Show("El campo de correo electrónico esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Regex.IsMatch(txtEmailUp.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Correo electrónico inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmailUp.Focus();
                return;
            }

            int proveedorID = Convert.ToInt32(txtIDUp.Text);
            Proveedores proveedor = _context.Proveedores.FirstOrDefault(q => q.ProveedorID.Equals(proveedorID));
            if (proveedor == null)
            {
                MessageBox.Show("El Proveedor no existe.");
            }
            proveedor.NombreProveedor = txtNombreUp.Text;
            proveedor.Telefono = maskTelUp.Text;
            proveedor.CorreoElectronico = txtEmailUp.Text;

            int rowsAffected = _context.SaveChanges();
            if (rowsAffected > 0) {
                               MessageBox.Show("Se ha actualizado el proveedor en la base de datos.");
            }
            this.cargaDatos();
            txtIDUp.Clear();
            txtNombreUp.Clear();
            maskTelUp.Clear();
            txtEmailUp.Clear();
        }

        private void View_Click(object sender, EventArgs e)
        {
           this.cargaDatos();
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

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {

                e.Handled = true;

            }
            else
            {
                e.Handled = false;

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

        private void txtEmailUp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {

                e.Handled = true;

            }
            else
            {
                e.Handled = false;

            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
