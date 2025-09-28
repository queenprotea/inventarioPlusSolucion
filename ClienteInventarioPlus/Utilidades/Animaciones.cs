﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ClienteInventarioPlus.Utilidades
{
    public class Animaciones
    {
        public static void IniciarAnimacion(UIElement elementoObjetivo, string claveRecurso, Action alCompletar = null)
        {
            if (elementoObjetivo == null || string.IsNullOrEmpty(claveRecurso)) return;

            var animacion = Application.Current.Resources[claveRecurso] as Storyboard;
            if (animacion != null)
            {
                var animacionClonada = animacion.Clone();

                if (alCompletar != null)
                    animacionClonada.Completed += (s, e) => alCompletar();

                if (elementoObjetivo is FrameworkElement elementoMarco)
                    animacionClonada.Begin(elementoMarco);
            }
        }
        public static void SacudirTextBox(TextBox textBox)
        {
            if (textBox == null) return;

            var gridPrincipal = (Grid)textBox.Template.FindName("MainGrid", textBox);

            if (gridPrincipal != null)
            {
                var guionAnimacion = Application.Current.Resources["ShakeAnimation"] as Storyboard;
                if (guionAnimacion != null)
                {
                    guionAnimacion.Begin(gridPrincipal, true);
                    return;
                }
            }

            if (textBox.RenderTransform == null || !(textBox.RenderTransform is TranslateTransform))
                textBox.RenderTransform = new TranslateTransform();

            IniciarAnimacion(textBox, "ShakeAnimation");
        }
        public static void SacudirPasswordBox(PasswordBox passwordBox)
        {
            if (passwordBox == null) return;

            if (passwordBox.RenderTransform == null || !(passwordBox.RenderTransform is TranslateTransform))
                passwordBox.RenderTransform = new TranslateTransform();

            IniciarAnimacion(passwordBox, "ShakeAnimation");
        }

        public static void SacudirDatePicker(DatePicker datePicker)
        {
            if (datePicker == null) return;

            if (datePicker.RenderTransform == null || !(datePicker.RenderTransform is TranslateTransform))
                datePicker.RenderTransform = new TranslateTransform();

            IniciarAnimacion(datePicker, "ShakeAnimation");
        }
    }
}