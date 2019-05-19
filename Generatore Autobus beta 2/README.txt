Il programma funziona in maniera del tutto automatica.
Quando un veicolo cerca di spedire un record (JsonDataRecord) tramite la classe Trasmission,
prima verifica che la rete sia attiva inviando un ping ad un indirizzo prestabilito,
una volta verificata la connessione si occupa di trasmettere il record
attraverso un POST verso il server specificato nelle preferenze.
Sempre in caso di presenza di rete,
vengono controllati l'eventuale presenza di record rimasti nello stack o su file e inviati.
Se, invece, la connessione non è operativa, il record viene passato alla classe Archive,
che prima la inserisce in uno stack, poi controlla quanti elementi ci sono in quest'ultimo,
se il numero è superiore al numero impostato nelle preferenze,
tutto lo stack viene salvato su file.

La funzione Main() del Program.cs esegue un test che simula il moto di un veicolo con la classe Vehicle.
Viene caricato il file di preferenze, dopodiché, in un loop, viene aggiornata l'istanza v1 che crea un percorso casuale.
All'inizio la trasmissione viene effettuata normalmente, poi all'interno del loop, viene simulata la perdita della connessione
per testare il salvataggio dei dati.
Una volta riattivata la connessione, successivamente nel loop, i nuovi ed i vecchi record vengono inviati.

Per effettuare la simulazione si devono avviare il file "server.js" nella cartella "testNodeServer", il database influx,
ed infine il main() stesso.