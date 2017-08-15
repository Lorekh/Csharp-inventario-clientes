using System;
using System.IO;

namespace LibreriaClases {
	public class directorio {
		public static void CrearDirectorio() {
			string path = ( Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Nostra_Inv" );

			if( !Directory.Exists(path) ) {
				Directory.CreateDirectory(path);
			} else {
				Console.Write("El directorio ya existe");
			}
		}
	}
}
