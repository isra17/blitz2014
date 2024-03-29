from thrift.transport import TSocket, TTransport
from thrift.protocol import TBinaryProtocol
import document
from indexer import Indexer
import json

def fill_value(client):
    artists = json.load(open("data/artist.json"))
    artist_list = []

    for artist in artists:
        artist_list.append(document.ttypes.Artist(
            id=artist["id"],
            name=artist["name"],
            origin=artist["origin"],
            active_start=artist["active_start"],
            active_end=artist["active_end"],
            genres=artist["genres"],
            labels=artist["labels"],
            albums=artist["albums"],
            group_names=artist["group_names"],
            instruments_played=artist["instruments_played"],
            text=artist["text"]))

    for artist in artist_list:
        client.indexArtist(artist)

host = "localhost"
#host = "192.168.220.154"
port = 9090

socket = TSocket.TSocket(host, port)
transport = TTransport.TBufferedTransport(socket)
protocol = TBinaryProtocol.TBinaryProtocol(transport)
client = Indexer.Client(protocol)
transport.open()

client.reset()
fill_value(client)

# search
query_tree_nodes = [
    document.ttypes.QueryTreeNode(
        value="OR", type=document.ttypes.NodeType.OPERATOR,
        leftPart=2, rightPart=3, id=1),
    document.ttypes.QueryTreeNode(
        value="2j", type=document.ttypes.NodeType.LITERAL,
        leftPart=-1, rightPart=-1, id=2),
    document.ttypes.QueryTreeNode(
        value="3j", type=document.ttypes.NodeType.LITERAL,
        leftPart=-1, rightPart=-1, id=3)
]

# search
query_tree_nodes = [
    document.ttypes.QueryTreeNode(
        value="4j", type=document.ttypes.NodeType.LITERAL,
        leftPart=-1, rightPart=-1, id=1)
]


query = document.ttypes.Query(rootId=1, queryTreeNodes=query_tree_nodes)
query_response = client.query(query)

print "Results:"
for doc in query_response.results:
    print doc.id, ": ", document.ttypes.DocumentType._VALUES_TO_NAMES[doc.documentType]

print "Facets:"
for facet in query_response.facets:
    print " ====", facet.metadataName, " ==== "
    for val in facet.values:
        print "-", val.value, ":", val.count


transport.close()