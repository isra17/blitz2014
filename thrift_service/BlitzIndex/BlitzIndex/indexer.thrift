
namespace java com.coveo.blitz.thrift
namespace csharp com.coveo.blitz.thrift

include "document.thrift"

/**
 * L'Indexer est le service responsable d'indexer les artistes et les albums pousses par Coveo.
 * Vous devez recevoir les artistes et les albums envoyes et les mettre dans votre index afin qu'ils
 * puissent etre recherches.
 * 
 * Lors que l'execution des appels de type "index" se termine, vous devez avoir complete l'indexation de l'artiste
 * ou de l'album et cet artiste ou ce document doit etre maintenant cherchable.
 */
service Indexer {
    /**
     * Demande a votre index d'indexer l'artiste passe en parametre.
     * La methode indexArtist doit retourner uniquement lorsque le processus d'indexation est complete.
     */
    void indexArtist(1: document.Artist artistToIndex),
    /**
     * Demande a votre index d'indexer l'album passe en parametre.
     * La methode indexAlbum doit retourner uniquement lorsque le processus d'indexation est complete.
     */
    void indexAlbum(1: document.Album albumToIndex),
    /**
     * Demande a votre index de retourner les resultats de la requete passee en parametre.
     * L'ordre dans lequel les resultats sont retournes est important.
     * 
     * Voir la documentation du concours pour savoir dans quel ordre les artistes et les albums doivent etre retournes.
     */
    document.QueryResponse query(1: document.Query query)
    
    /**
     * Demande a votre index de se reinitialiser. Tout le contenu de l'index doit etre efface.
     * Cela permet de s'assurer que votre index ne garde pas des documents en trop entre deux evaluations, ce qui
     * nuirait a votre score.
     */
    void reset()
    
    /**
     * Verifie si votre index est operationel et pret a repondre.
     */
    void ping()
    
    document.Album GetAlbum(1: string id)
    
    document.Artist GetArtist(1: string id)
}
