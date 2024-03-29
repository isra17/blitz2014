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
port = 9090

socket = TSocket.TSocket(host, port)
transport = TTransport.TBufferedTransport(socket)
protocol = TBinaryProtocol.TBinaryProtocol(transport)
client = Indexer.Client(protocol)
transport.open()

client.reset()
fill_value(client)

transport.close()