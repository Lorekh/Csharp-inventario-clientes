using System;
using System.Windows;
using LibreriaClases;
using System.Globalization;

namespace NostraWPF {
	/// <summary>
	/// Lógica de interacción para VAgregarInventario.xaml
	/// </summary>
	public partial class VAgregarInventario: Window {
		DB miDB = new DB();

		public VAgregarInventario() {
			InitializeComponent();
 		}

		private void buttonCancelar_Click(object sender, RoutedEventArgs e) {
            Hide();
		}

		private void buttonAgregar_Click(object sender, RoutedEventArgs e) {
			TextInfo ProperCase = new CultureInfo("en-US", false).TextInfo;

			string articulo = textBoxArticulo.Text;
			articulo = ProperCase.ToTitleCase(articulo.ToLower());
			string cantidad = textBoxCantidad.Text;
			int Icantidad;

			if( !miDB.verificarArticulo(articulo) ) {
				if( Int32.TryParse(cantidad, out Icantidad) ) {
					if (Icantidad == 0) {
						label_estado.Content = "No se pueden agregar articulos sin cantidad";
					} else {
						miDB.AgregarInventario(articulo, Icantidad);
						label_estado.Content = articulo + " agregado";

                        // Se deberia llamar funcion para actualizar datagrid del mainwindow

                        textBoxArticulo.Text = string.Empty;
						textBoxCantidad.Text = string.Empty;
					}
				} else {
					label_estado.Content = "La cantidad debe ser un Numero";
				}
			} else {
				label_estado.Content = "El Articulo ya existe";
				textBoxArticulo.Text = string.Empty;
				textBoxCantidad.Text = string.Empty;
			}
		}
	}
}
