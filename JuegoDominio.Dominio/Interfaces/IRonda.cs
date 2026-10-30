namespace JuegoDominio.Dominio.Interfaces
{
    public interface IRonda
    {
        void Suscribir(IObservadorRonda o);
        void Desuscribir(IObservadorRonda o);
    }
}
