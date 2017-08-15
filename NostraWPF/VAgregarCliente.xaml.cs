using System;
using System.Globalization;
using System.Windows;
using LibreriaClases;

namespace NostraWPF
{
    /// <summary>
    /// Lógica de interacción para VAgregarCliente.xaml
    /// </summary>
    public partial class VAgregarCliente: Window {
		public VAgregarCliente() {
			InitializeComponent();
		}

		private void buttonCancelar_Click(object sender, RoutedEventArgs e) {
            Hide();
		}

        private void buttonAgregar_Click_1(object sender, RoutedEventArgs e)
        {
            DB miDB = new DB();
            TextInfo ProperCase = new CultureInfo( "en-US", false ).TextInfo;

            string nombre = textBoxNombre.Text;
            nombre = ProperCase.ToTitleCase( nombre.ToLower() );
            string apellido = textBoxApellido.Text;
            apellido = ProperCase.ToTitleCase( apellido.ToLower() );
            string tlflocal = textBoxLocal.Text;
            int local, movil;
            string tlfmovil = textBoxMovil.Text;
            string correo = textBoxCorreo.Text;
            string nombreCompleto = nombre + " " + apellido;

            if( ( nombre.Length < 3 ) || ( apellido.Length < 3 ) ) {
                label_estado.Content = "No se pueden introducir clientes sin nombre o apellido";
            } else {
                if( !miDB.verificarCliente( nombre, apellido ) ) {
                    if( Int32.TryParse( tlflocal, out local ) && ( Int32.TryParse( tlfmovil, out movil ) ) ) {
                        label_estado.Content = tlflocal.Length < 7 ? "El telefono local le faltan numeros" : ( tlflocal.Length > 11 ? "El telefono local tiene numeros de mas" : "" );
                    } else {
                        miDB.AgregarCliente( nombre, apellido, tlflocal, tlfmovil, correo );
                        label_estado.Content = nombreCompleto + " Agregado";

                        // Se deberia llamar funcion par actualizar datagrid en mainwindow

                        textBoxNombre.Text = string.Empty;
                        textBoxApellido.Text = string.Empty;
                        textBoxLocal.Text = string.Empty;
                        textBoxMovil.Text = string.Empty;
                        textBoxCorreo.Text = string.Empty;
                    }
                } else {
                    label_estado.Content = "El Cliente ya existe";

                    textBoxNombre.Text = string.Empty;
                    textBoxApellido.Text = string.Empty;
                    textBoxLocal.Text = string.Empty;
                    textBoxMovil.Text = string.Empty;
                    textBoxCorreo.Text = string.Empty; 
                }
            }
        } // Fin boton agregar cliente
	}
}
