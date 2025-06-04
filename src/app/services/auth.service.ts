import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Usuario } from '../models/usuario.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5242/api/Auth';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { email, password });
  }

  register(usuario: Usuario): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, usuario);
  }

  guardarToken(token: string) {
    localStorage.setItem('token', token);
  }

  obtenerToken(): string | null {
    return localStorage.getItem('token');
  }

  guardarUsuario(usuario: any) {
    localStorage.setItem('usuario', JSON.stringify(usuario));
  }

  obtenerUsuario(): any {
    const user = localStorage.getItem('usuario');
    return user ? JSON.parse(user) : null;
  }

  cerrarSesion() {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
  }
}
