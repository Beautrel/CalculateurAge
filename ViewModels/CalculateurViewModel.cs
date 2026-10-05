namespace CalculateurAge.ViewModels;

// Contient l'ÉTAT de l'écran et les ACTIONS possibles.
public class CalculateurViewModel : BaseViewModel
{
    private const int AgeMajorite = 18;

    // Champs privés : la vraie donnée.
    private string _nom = "";
    private DateTime _dateNaissance = DateParDefaut();
    private string _resultat = "";
    private string _message = "";
    private string _messageAnniversaire = "";
    private bool _resultatVisible;

    // Propriétés publiques : ce que le XAML voit.
    public string Nom
    {
        get => _nom;
        set { if (SetField(ref _nom, value))
                  CalculerCommand.Rafraichir(); }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set => SetField(ref _dateNaissance, value);
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    // Fonctionnalité 1 : "Majeur" ou "Mineur".
    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    // Fonctionnalité 3 : jours avant le prochain anniversaire.
    public string MessageAnniversaire
    {
        get => _messageAnniversaire;
        set => SetField(ref _messageAnniversaire, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    // Liées à Button.Command dans le XAML.
    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom));

        // Fonctionnalité 2 : toujours disponible.
        EffacerCommand = new RelayCommand(Effacer);
    }

    private static DateTime DateParDefaut()
        => DateTime.Today.AddYears(-20);

    // Date de l'anniversaire dans une année donnée.
    // Né un 29 février : le 28 février les années non bissextiles.
    private static DateTime AnniversaireEn(int annee, DateTime naissance)
    {
        int jour = naissance.Day;
        if (naissance.Month == 2 && naissance.Day == 29
            && !DateTime.IsLeapYear(annee))
            jour = 28;
        return new DateTime(annee, naissance.Month, jour);
    }

    // Nombre de jours avant le prochain anniversaire (0 = aujourd'hui).
    private static int JoursAvantAnniversaire(DateTime naissance)
    {
        DateTime aujourdhui = DateTime.Today;
        DateTime prochain = AnniversaireEn(aujourdhui.Year, naissance);
        if (prochain < aujourdhui)
            prochain = AnniversaireEn(aujourdhui.Year + 1, naissance);
        return (prochain - aujourdhui).Days;
    }

    // La logique métier : aucun contrôle d'interface ici.
    private void Calculer()
    {
        int age = DateTime.Today.Year
                  - DateNaissance.Year;
        if (DateNaissance.Date >
            DateTime.Today.AddYears(-age)) age--;

        Resultat = $"{Nom}, vous avez {age} ans";
        Message = age >= AgeMajorite ? "Majeur" : "Mineur";

        int jours = JoursAvantAnniversaire(DateNaissance);
        MessageAnniversaire = jours == 0
            ? "Joyeux anniversaire !"
            : $"Prochain anniversaire dans {jours} jour(s)";

        ResultatVisible = true;
    }

    // Remet tous les champs à zéro.
    private void Effacer()
    {
        Nom = "";   // regrise aussi le bouton Calculer
        DateNaissance = DateParDefaut();
        Resultat = "";
        Message = "";
        MessageAnniversaire = "";
        ResultatVisible = false;
    }
}