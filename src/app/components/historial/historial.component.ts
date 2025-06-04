import { Component, OnInit } from '@angular/core';
import { ReservaService } from '../../services/reserva.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-historial',
  standalone: true,
  templateUrl: './historial.component.html',
  styleUrls: ['./historial.component.scss'],
  imports: [CommonModule]
})
export class HistorialComponent implements OnInit {
  historial: any[] = [];

  constructor(
    private reservaService: ReservaService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const usuario = this.authService.obtenerUsuario();
    if (!usuario) {
      alert('Usuario no autenticado');
      return;
    }

    this.reservaService.getHistorial(usuario.id).subscribe(data => {
      this.historial = data;
    });
  }

  cancelarReserva(reservaId: number): void {
    if (!confirm('¿Estás seguro de cancelar esta reserva?')) return;

    this.reservaService.cancelarReserva(reservaId).subscribe({
      next: () => {
        alert('Reserva cancelada ✅');
        this.ngOnInit(); // recargar el historial
      },
      error: () => {
        alert('Error al cancelar ❌');
      }
    });
  }
}
