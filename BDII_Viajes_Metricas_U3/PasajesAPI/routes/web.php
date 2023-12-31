<?php

use Illuminate\Support\Facades\Route;
//
use App\Http\Controllers\PrometheusMetricsController;

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider and all of them will
| be assigned to the "web" middleware group. Make something great!
|
*/

Route::get('/', function () {
    return view('welcome');
});

//Aqui estaban tus rutas, ahora las pase al archivo "api.php"

//Endpoint /metrics
Route::get('/metrics', [PrometheusMetricsController::class, 'index']);//getMetrics