// src/app/services/reserva.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReservaService {
  private apiUrl = 'http://localhost:5242/api/Reserva';

  constructor(private http: HttpClient) {}

  getHistorial(usuarioId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/usuario/${usuarioId}`);
  }

  cancelarReserva(reservaId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/cancelar/${reservaId}`, {});
  }
}
