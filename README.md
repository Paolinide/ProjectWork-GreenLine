Come usare il programma:

(Per testare il funzionamento del sistema, influx 1.4.7-1 deve essere presente sulla macchina)
1: avviare database "influxd.exe"
2: avviare la console "influx.exe"
3: aprire visual studio code i progetti

    - testNodeServer
    - Generatore Autobus beta 2

4: avviare prima testNodeServer
5: avviare Generatore Autobus beta 2
6: avviare postman (per verificare il contenuto del body dei post)
7: select da console influx per verificare l'inserimento dei dati
( selct * from "response_times" )
8: fare GET all'indirizzo http://localhost:3000/get per verificare body 