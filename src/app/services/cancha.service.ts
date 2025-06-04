import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CanchaService {
  private apiUrl = 'http://localhost:5242/api';

  constructor(private http: HttpClient) {}

  getCanchas(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Cancha`);
  }

  getHorariosPorCancha(canchaId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Horario/porCancha/${canchaId}`);
  }

  getBloquesHorario(canchaId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Horario/bloques/cancha/${canchaId}`);
  }
  crearReserva(reserva: any): Observable<any> {
  return this.http.post(`${this.apiUrl}/Reserva`, reserva);
}
getHistorialReservas(usuarioId: number): Observable<any[]> {
  return this.http.get<any[]>(`${this.apiUrl}/Reserva/usuario/${usuarioId}`);
}

cancelarReserva(reservaId: number): Observable<any> {
  return this.http.put(`${this.apiUrl}/Reserva/cancelar/${reservaId}`, {});
}
getHistorial(usuarioId: number): Observable<any[]> {
  return this.http.get<any[]>(`${this.apiUrl}/Reserva/usuario/${usuarioId}`);
}


}
