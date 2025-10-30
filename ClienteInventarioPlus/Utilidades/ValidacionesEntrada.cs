using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ClienteInventarioPlus.Utilidades
{
    public class ValidacionesEntrada
    {
        public static string ValidarNombre(TextBox textBox)
        {
            string nombre = textBox.Text;

            if (string.IsNullOrWhiteSpace(nombre)) {
                Animaciones.SacudirTextBox(textBox);
                return "El nombre no puede estar vacío.";
            }
            if (nombre.Length < 2) {
                Animaciones.SacudirTextBox(textBox);
                return "El nombre es demasiado corto.";
            }
            return null;
        }

        public static string ValidarTelefono(TextBox textBox)
        {
            string telefono = textBox.Text;

            if (string.IsNullOrWhiteSpace(telefono)) {
                Animaciones.SacudirTextBox(textBox);
                return "El teléfono no puede estar vacío.";
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(telefono, @"^\+?\d{8,13}$")) {
                Animaciones.SacudirTextBox(textBox);
                return "El teléfono no tiene un formato válido.";
            }
            return null;
        }

        public static string ValidarPassword(PasswordBox passwordBox)
        {
            string pass = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(pass)) {
                Animaciones.SacudirPasswordBox(passwordBox);
                return "La contraseña no puede estar vacía.";
            }
            if (pass.Length < 5) {
                Animaciones.SacudirPasswordBox(passwordBox);
                return "La contraseña es demasiado corta.";
            }
            return null;
        }

        public static string ValidarPasswordTextBox(TextBox textBox)
        {
            string pass = textBox.Text;

            if (string.IsNullOrWhiteSpace(pass))
            {
                Animaciones.SacudirTextBox(textBox);
                return "La contraseña no puede estar vacía.";
            }
            if (pass.Length < 5)
            {
                Animaciones.SacudirTextBox(textBox);
                return "La contraseña es demasiado corta.";
            }
            return null;
        }

        public static string ValidarFecha(DatePicker datePicker)
        {
            DateTime? fecha = datePicker.SelectedDate;

            if (fecha == null) {
                Animaciones.SacudirDatePicker(datePicker);
                return "Debe seleccionar una fecha.";
            }
            if (fecha.Value > DateTime.Now) {
                Animaciones.SacudirDatePicker(datePicker);
                return "La fecha no puede estar en el futuro.";
            }
            if (fecha.Value.Year < 1900) {
                Animaciones.SacudirDatePicker(datePicker);
                return "Fecha invalida";
            }
            return null;
        }

        public static string ValidarCorreo(TextBox textBox)
        {
            string correo = textBox.Text;

            if (string.IsNullOrWhiteSpace(correo))
            {
                Animaciones.SacudirTextBox(textBox);
                return "El correo no puede estar vacío.";
            }

            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(correo, patron))
            {
                Animaciones.SacudirTextBox(textBox);
                return "El correo no tiene un formato válido.";
            }
            return null;
        }
        public static string ValidarNombreUsuario(TextBox textBox)
        {
            string nombreUsuario = textBox.Text;

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                Animaciones.SacudirTextBox(textBox);
                return "El nombre de usuario no puede estar vacío.";
            }
            if (nombreUsuario.Length < 3)
            {
                Animaciones.SacudirTextBox(textBox);
                return "El nombre de usuario es demasiado corto.";
            }

            return null;
        }
        
        public static string ValidarRadioButtons(RadioButton rb1, RadioButton rb2)
        {
            if (rb1.IsChecked != true && rb2.IsChecked != true)
            {
                return "Debe seleccionar una opción.";
            }

            return null;
        }


        public static string ValidarNumero(TextBox textBox)
        {
            string texto = textBox.Text;

            // Validar que no esté vacío
            if (string.IsNullOrWhiteSpace(texto))
            {
                Animaciones.SacudirTextBox(textBox);
                return "Ingresa un numero.";
            }

            // Validar que sea un número entero
            if (!int.TryParse(texto, out int numero))
            {
                Animaciones.SacudirTextBox(textBox);
                return "Debe ser un número entero.";
            }

            // Validar que sea positivo
            if (numero <= 0)
            {
                Animaciones.SacudirTextBox(textBox);
                return "Debe ser mayor que cero.";
            }

            return null; // ✅ Sin errores
        }


        public static void ValidarEntrada(TextBox textBox, string patron, int longitudMaxima)
        {
            textBox.TextChanged += (s, e) =>
            {
                string entrada = textBox.Text;
                string limpiado = Regex.Replace(entrada, patron, "");

                if (limpiado.Length > longitudMaxima)
                    limpiado = limpiado.Substring(0, longitudMaxima);

                if (entrada != limpiado)
                {
                    textBox.Text = limpiado;
                    textBox.SelectionStart = limpiado.Length;
                    Animaciones.SacudirTextBox(textBox);
                }
            };
        }
        public static void ValidarEntradaContrasena(PasswordBox passwordBox, string patron, int longitudMaxima)
        {
            passwordBox.PasswordChanged += (s, e) =>
            {
                string entrada = passwordBox.Password;
                string limpiado = Regex.Replace(entrada, patron, "");

                if (limpiado.Length > longitudMaxima)
                    limpiado = limpiado.Substring(0, longitudMaxima);

                if (entrada != limpiado)
                {
                    passwordBox.Password = limpiado;
                    Animaciones.SacudirPasswordBox(passwordBox);
                }
            };
        }
        
        public static string ValidarDireccion(TextBox textBox)
        {
            string direccion = textBox.Text;

            if (string.IsNullOrWhiteSpace(direccion))
            {
                Animaciones.SacudirTextBox(textBox);
                return "La dirección no puede estar vacía.";
            }

            return null; // válido
        }

        
    }
}