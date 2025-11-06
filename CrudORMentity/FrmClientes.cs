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
    public partial class FrmClientes : Form
    {
        private Actividad1Entities _context;

        public FrmClientes()
        {
            InitializeComponent();
        }

        private void cargarDatos()
        {
            var listaClientes = _context.Clientes
                   .Select(p => new {
                       ID = p.ClienteID,
                       Nombre_Cliente = p.NombreCompleto,
                       CorreoElectrónico_Cliente = p.CorreoElectronico,
                       Teléfono_Cliente = p.Telefono,
                       Dirección_Cliente = p.Direccion,

                   }).ToList();

            dgv.DataSource = listaClientes;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void FrmClientes_Load(object sender, EventArgs e)
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
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("El campo de correo electrónico esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Correo electrónico inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
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
                maskTel.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtDir.Text))
            {
                MessageBox.Show("El campo de dirección esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            Clientes cliente = new Clientes()
            {
                NombreCompleto = txtNombre.Text,
                CorreoElectronico = txtEmail.Text,
                Telefono = maskTel.Text,
                Direccion = txtDir.Text,

            };

            _context.Clientes.Add(cliente);

            int rowsAffected = _context.SaveChanges();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Se ha insertado al cliente en la base de datos.");
            }

            this.cargarDatos();
            txtNombre.Clear();
            txtEmail.Clear();
            maskTel.Clear();
            txtDir.Clear();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Debe introducir un ID válido.");
                return;
            }
            if (MessageBox.Show("Se eliminarán también las facturas del cliente. ¿Desea continuar?", "Advertencia", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            else
            {

                int clienteID = Convert.ToInt32(txtID.Text);

                Clientes cliente = _context.Clientes.FirstOrDefault(q => q.ClienteID.Equals(clienteID));
                if (cliente == null)
                {
                    MessageBox.Show($"El Cliente no existe.");
                    return;
                }
                foreach (var factura in cliente.Facturas.ToList())
                {
                    _context.DetallesFactura.RemoveRange(factura.DetallesFactura);
                }

                _context.Facturas.RemoveRange(cliente.Facturas);
              
                _context.Clientes.Remove(cliente);
                int rowsAffected = _context.SaveChanges();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Se ha eliminado el cliente en la base de datos.");
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
            if (string.IsNullOrEmpty(txtDirUp.Text))
            {
                MessageBox.Show("El campo de dirección esta vacío o es incorrecto", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (!int.TryParse(txtIDUp.Text, out int clienteId))
            {
                MessageBox.Show("ID inválido.");
                return;
            }

            int clienteID = Convert.ToInt32(txtIDUp.Text);

            Clientes cliente = _context.Clientes.FirstOrDefault(q => q.ClienteID.Equals(clienteID));
            if (cliente == null)
            {
                MessageBox.Show("El Cliente no existe.");
                return;
            }

            cliente.NombreCompleto = txtNombreUp.Text;
            cliente.CorreoElectronico = txtEmailUp.Text;
            cliente.Telefono = maskTelUp.Text;
            cliente.Direccion = txtDirUp.Text;

            int rowsAffected = _context.SaveChanges();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Se ha actualizado el cliente en la base de datos.");
            }

            this.cargarDatos();
            txtIDUp.Clear();
            txtNombreUp.Clear();
            txtEmailUp.Clear();
            maskTelUp.Clear();
            txtDirUp.Clear();

        }

        private void View_Click(object sender, EventArgs e)
        {
            this.cargarDatos();
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

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
