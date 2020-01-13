using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private CLIENTE nuevoCliente;
        private InformesEntities contexto;
        public MainWindow()
        {
            InitializeComponent();
            contexto = new InformesEntities();
            nuevoCliente = new CLIENTE();

            contexto.CLIENTES.Load();

            DatosListBox.DataContext = contexto.CLIENTES.Local;
            InsertarStackPanel.DataContext = nuevoCliente;
            EliminarDatosComboBox.DataContext = contexto.CLIENTES.Local;
            ModificarStackPanel.DataContext = contexto.CLIENTES.Local;
        }

        private void InsertarButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(IdentificadorTextBox.Text) > contexto.CLIENTES.Count())
            {
                contexto.CLIENTES.Add(nuevoCliente);
                contexto.SaveChanges();
                nuevoCliente = new CLIENTE();
                InsertarStackPanel.DataContext = nuevoCliente;
            }
            else MessageBox.Show("El campo 'Identificador' ya existe", "Insertar Dato", MessageBoxButton.OK, MessageBoxImage.Exclamation);


        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            CLIENTE deleteCliente = (CLIENTE)EliminarDatosComboBox.SelectedItem;

            MessageBoxResult boxResult = MessageBox.Show("Eliminar " + deleteCliente.nombre + " " + deleteCliente.apellido + "?", "Eliminar Dato", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (boxResult == MessageBoxResult.OK)
            {
                contexto.CLIENTES.Remove((CLIENTE)EliminarDatosComboBox.SelectedItem);
                contexto.SaveChanges();
            }
            else MessageBox.Show("No se ha eliminado " + deleteCliente.nombre + " " + deleteCliente.apellido, "Eliminar Dato", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void ModificarButton_Click(object sender, RoutedEventArgs e)
        {
            CLIENTE updateCliente = (CLIENTE)ModificarDatosComboBox.SelectedItem;

            MessageBoxResult boxResult = MessageBox.Show("¿Actualizar " + updateCliente.nombre + " " + updateCliente.apellido + "?", "Actualizar Dato", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (boxResult == MessageBoxResult.OK)
            {
                updateCliente.nombre = NombreModificarTextBox.Text;
                updateCliente.apellido = ApellidoModificarTextBox.Text;
                contexto.SaveChanges();
                MessageBox.Show("Actualizado correctamente", "Actualizar Dato", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else MessageBox.Show("No se ha actualizado " + updateCliente.nombre + " " + updateCliente.apellido, "Actualizar Dato", MessageBoxButton.OK, MessageBoxImage.Information);


        }
    }
}
