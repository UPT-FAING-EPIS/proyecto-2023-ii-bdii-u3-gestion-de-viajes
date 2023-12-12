<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use SqsSimple\SqsMessenger;
use SqsSimple\SqsWorker;
use Illuminate\Support\Facades\Http;

class PasajesController extends Controller
{

    private $couch = [
        'usuario'   =>  'ubuntu',
        'clave'     =>  'ubuntu',
        'database'  =>  'ventapasajes',
        'server'    =>  '172.233.215.194:5984'
    ];
    //CONEXION SQS CON AMAZON

    private $AwsConfig = [
        'AWS_KEY'=>'AKIARXAGXFVOCXMOJBW4', //You should put your AWS_KEY
        'AWS_SECRET_KEY'=>'MbYRvsMkqW2VFhJrOpQ4wIncvcd2KrI1cxp4LVEd', //You should put your AWS_SECRET_KEY
        'AWS_REGION'=>'us-east-1', //You should put your AWS_REGION
        'API_VERSION'=>'2012-11-05'
    ];

    private $queueUrl = "https://sqs.us-east-1.amazonaws.com/118125702492/enviomensajes";

    public function registrar(Request $request){

        //print_r($request->all());
        //return;

        /**
         * valores del objeto json
         * - cliente_nombre
         * - cliente_dni
         * - cliente_telefono
         * - cliente_correo
         * - fecha_hora_viaje
         * - fecha_hora_viaje
         * - destino
         * - origen
         */
        //YA DENTRO LO DESDOBLO Y LO DESGLOSO
        $data = [];
        $data['cliente'] = [];
        $data['cliente']['nombre'] = $request->get("cliente_nombre");
        $data['cliente']['dni'] = $request->get("cliente_dni");
        $data['cliente']['telefono'] = $request->get("cliente_telefono");
        $data['cliente']['correo'] = $request->get("cliente_correo");
        $data['fecha_hora_viaje'] = $request->input("fecha_hora_viaje");
        $data['numero_asiento'] = $request->input("numero_asiento");
        $data['destino'] = $request->input("destino");
        $data['origen'] = $request->input("origen");

        // convertimos el mensaje en un json json
        $data_as_json = json_encode($data);

        // enviamos el mensaje a la cola de mensajes en aws sqs
        $messenger = new SqsMessenger($this->AwsConfig);
        $message = $data_as_json;
        $messageAttributes = [];
        $delaySeconds = 10; // (Not FIFO Queue type) - The time in seconds that the delivery of all messages in the queue will be delayed. An integer from 0 to 900 (15 minutes). The default for this attribute is 0 (zero).
        $messageGroupId = ''; // (FIFO Queue type) - The tag that specifies that a message belongs to a specific message group. Messages that belong to the same message group are always processed one by one, in a strict order relative to the message group (however, messages that belong to different message groups might be processed out of order).
        $messageDeduplicationId = ''; // (FIFO Queue type) - The token used for deduplication of sent messages. If a message with a particular message deduplication ID is sent successfully, any messages sent with the same message deduplication ID are accepted successfully but aren't delivered during the 5-minute deduplication interval. The queue should either have ContentBasedDeduplication enabled or MessageDeduplicationId provided explicitly.
        $result = $messenger->publish( $this->queueUrl, $message, $messageAttributes, $delaySeconds, $messageGroupId, $messageDeduplicationId);

        echo $this->guardarEnDB($data);
    }

    private function getServerURL(){
        $endpoint = "http://".$this->couch['usuario'].":".$this->couch['clave']."@".$this->couch['server']."/".$this->couch['database']."/";
        return $endpoint;
    }
    // Y CUANDO ESTO PASE SE GUARDA EN COUCHDB
    private function guardarEnDB($json){

        $response = Http::post($this->getServerURL(), $json);
        print_r($response->getBody()->getContents());
    }

    public function listar(){
        $response = Http::get($this->getServerURL() . "_all_docs");
        return $response->getBody()->getContents();
    }

}
