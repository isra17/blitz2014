
namespace java com.coveo.blitz.thrift
namespace csharp com.coveo.blitz.thrift


/**
 * Les types de documents qui peuvent etre retournes par l'Indexer
 */ 
enum DocumentType {
    /**
     * Signifie qu'un document contient des informations sur un artiste.
     */
    ARTIST = 1,
    /**
     * Signifie qu'un document contient des informations sur un album.
     */
    ALBUM = 2,
}

/**
 * Les type de noeud dans l'arbre syntaxique de la requete.
 */
enum NodeType {
    /**
     * Un noeud qui represente un operateur de requete (AND, OR, ...)
     * "value" contient l'operateur.
     * "leftPart" contient la partie a droite de l'operateur.
     * "rightPart" contient la partie de gauche de l'operateur.
     */
    OPERATOR = 1,
    /**
     * Un noeud qui represente un terme de la requete ("Madonna", "Justin", "Hello World")
     * "value" contient le terme.
     * "leftPart" est null.
     * "rightPart" est null.
     */
    LITERAL = 2,
}

/**
 * Un artiste qui fait de la musique.
 * Pensez a Justin Bieber.
 */
struct Artist {
    /**
     * L'identifiant unique pour cet artiste
     */
     1: string id
     /**
      * Le nom sous lequel l'artiste est connu
      */
     2: set<string> name
     /**
      * Le pays d'origine de l'artiste
      */
     3: set<string> origin
     /**
      * La date a laquelle l'artiste a commence a etre actif
      * La date est du format yyyy-MM-dd'T'HH:mm:ssZZZ
      * Voir la documentation pour plus d'information sur les formats de date
      */
     4: set<string> active_start
      /**
       * La date a laquelle l'artiste a cesse d'etre actif
       * La date est du format yyyy-MM-dd'T'HH:mm:ssZZZ
       * Voir la documentation pour plus d'information sur les formats de date
       */
     5: set<string> active_end
      /**
       * Les genre musicaux de cet artiste
       */
     6: set<string> genres
      /**
       * Les maisons de disque sous lesquelles cet artiste publie
       */
     7: set<string> labels
      /**
       * Les identifiants uniques des albums de cet artiste
       */
     8: set<string> albums
      /**
       * Le nom des groupes dont cet artiste est membre
       */
     9: set<string> group_names
      /**
       * Les instruments que cet artiste joue
       */
    10: set<string> instruments_played
      /**
       * La description de cet artiste.
       * La description est extraite de Wikipedia et contient de la 
       * syntaxe Mediawiki http://www.mediawiki.org/wiki/Help:Formatting
       */ 
    11: string text
}

/**
 * Un album de musique qui contient des chansons.
 * Pensez .. au dernier album de Justin Bieber.
 */
struct Album {
    /**
     * L'identifiant unique de cet album
     */
    1: string id
    /**
     * Le nom de cet album
     */
    2: set<string> name
    /**
     * Les identifiants uniques des artistes qui ont produit cet album
     */
    3: set<string> artists
    /**
     * La date a laquelle cet album a ete lance
     * La date est du format yyyy-MM-dd'T'HH:mm:ssZZZ
     * Voir la documentation pour plus d'information sur les formats de date
     */
    4: set<string> release_date
    /**
     * Les genre musicaux de cet album
     */
    5: set<string> genres
    /**
     * Les titres des chansons sur cet album
     */
    6: set<string> track_names
      /**
       * La description de cet album.
       * La description est extraite de Wikipedia et contient de la 
       * syntaxe Mediawiki http://www.mediawiki.org/wiki/Help:Formatting
       */ 
    7: string text
}

/**
 * Filtre sur une facette. Vous devez uniquement retourner des resultats qui, pour les metadata 
 * fournis contiennent une des valeurs fournies. Voir la documentation pour plus d'information.
 */
struct FacetFilter {
    /**
     * Le nom du metadata sur lequel le filtre s'applique.
     */
	1: string metadataName
	
	/**
	 * Les valeurs acceptees pour ce metadata.
	 */
	2: list<string> values
}

/**
 * Represente un noeud dans l'arbre syntaxique de la requete.
 * 
 * Voir la documentation pour des exemples.
 */
struct QueryTreeNode {
    /**
     * La valeur du noeud.
     * 
     * Par exemple, un noeud dont le type est "OPERATOR" pourrait avoir la valeur "OR",
     * "AND" ou "NOT". Un noeud dont le type est "LITERAL" pourrait avoir la valeur "Justin Bieber".
     */
	1: string value
	/**
     * Le type du noeud. Le type influence ce que sera contenu dans "value", "leftPart" et "rightPart".
     * Voir la documentation pour plus d'information et des exemples.
     */
	2: NodeType type
    /**
     * L'identifiant du noeud representant la partie de gauche de la requete. Si la valeur est -1, il n'y a pas de partie de gauche.
     */
	3: i32 leftPart
    /**
     * L'identifiant du noeud representant la partie de droite de la requete. Si la valeur est -1, il n'y a pas de partie de droite.
     */
	4: i32 rightPart
    
    /**
     * L'identifiant de ce noeud.
     * Est necessairement plus grand que zero.
     */
    5: i32 id
}

/**
 * Une "Query" est une requete faite a votre index. Votre index doit repondre de maniere appropriee
 * et le plus rapidement possible. 
 * 
 * Voir la documentation du concours pour savoir ce que signifie une reponse "appropriee".
 */
struct Query {
    /**
     * L'identifiant du noeud racine de l'arbre syntaxique de cette requete.
     */
    1: i32 rootId
    /**
     * Liste des noeuds composant l'arbre.
     */
    2: list<QueryTreeNode> queryTreeNodes
    
    /**
     * Liste de filtres sur les facettes
     */
    3: list<FacetFilter> facetFilters
}

/**
 * Un resultat d'une requete a votre index.
 * Les resultats doivent etre retournes dans le bon ordre.
 * Un resultat peut etre soit un Artist ou un Album. 
 * Si documentType est DocumentType.ARTIST, l'identifiant doit etre un identifiant d'artiste.
 * Si documentType est DocumentType.ALBUM, l'identifiant doit etre un identifiant d'album.
 *
 * Voir la documentation du concours pour savoir ce que signifie "dans le bon ordre".
 */
struct QueryResult {
    /**
     * Le type du document.
     * Si documentType est DocumentType.ARTIST, seul artist doit etre rempli.
     * Si documentType est DocumentType.ALBUM, seul album doit etre rempli.
     */ 
    1: DocumentType documentType
    
    /**
     * L'identifiant unique du document tel que fournit lors de l'indexation.
     */
    2: string id
}

/**
 * Une valeur pour une facet.
 *
 * Par exemple, si on a une facet sur le metadata "Auteur", une valeur 
 * pourrait etre "Nickleback" et un count "42".
 */
struct FacetValue {
    /**
     * Une valeur de facet.
     */
    1: string value
    
    /**
     * Le nombre de resultats qui ont cette valeur pour ce metadata.
     */
    2: i32 count
}

/**
 * Une facet et ses valeurs.
 */
struct FacetResult {
    /**
     * Le nom du metadata sur lequel la facet est basee (e.g. "Auteur")
     */ 
    1: string metadataName
    /**
     * La liste de valeurs associees a ce metadata (e.g. "Justin Bieber" : 5, "Bob Marley" : 3)
     */ 
    2: list<FacetValue> values
}


/**
 * Le resultat complet d'une requete a votre index.
 */
struct QueryResponse {
    /**
     * Toutes les facets pour les resultats obtenus par la requete.
     *
     * Voir la documentation du concours pour plus de details.
     */
    1: list<FacetResult> facets
    
    /**
     * Tous les resultats de la requete.
     *
     * Les resultats doivent etre retournes dans le bon ordre.
     * Voir la documentation du concours pour plus de details.
     */
    2: list<QueryResult> results

}

