using CalculateurAge.Views;

namespace CalculateurAge;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    // Gestionnaire appelé au clic sur le bouton Calculer.
    // sender = le contrôle cliqué ; e = données de l'évènement.
    private async void OnCalculerClicked(object sender, EventArgs e)
    {
        // Validation : on refuse un nom vide.
        if (string.IsNullOrWhiteSpace(entryNom.Text))
        {
            await DisplayAlertAsync("Erreur", "Entrez un nom", "OK");
            return; // on sort sans rien calculer
        }

        // DatePicker.Date est nullable dans cette version de MAUI.
        DateTime d = pickerDate.Date ?? DateTime.Today;
        int age = DateTime.Today.Year - d.Year;

        // Si l'anniversaire n'est pas encore passé cette année, on retire une année.
        if (d.Date > DateTime.Today.AddYears(-age)) age--;

        // Phase B : on navigue vers la page Résultat avec des paramètres.
        await Shell.Current.GoToAsync(
            $"{nameof(ResultatPage)}?nom={entryNom.Text}&age={age}");
    }
}