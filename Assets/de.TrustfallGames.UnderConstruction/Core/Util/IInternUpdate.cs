namespace de.TrustfallGames.UnderConstruction.Util {
    public interface IInternUpdate {
        void InternUpdate();

        void RegisterInternUpdate();

        void Init();

        void OnDestroy();
    }
}