import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CanchaService } from '../../services/cancha.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-horarios',
  standalone: true,
  templateUrl: './horarios.component.html',
  styleUrls: ['./horarios.component.scss'],
  imports: [CommonModule]
})
export class HorariosComponent implements OnInit {
  canchaId: number = 0;
  horarios: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private canchaService: CanchaService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.canchaId = +params['id'];
      this.cargarHorarios();
    });
  }

  cargarHorarios(): void {
    this.canchaService.getBloquesHorario(this.canchaId).subscribe(data => {
      this.horarios = data;
    });
  }

  reservarHorario(horario: any): void {
    if (horario.estado !== 'disponible') {
      alert('Este horario ya está ocupado.');
      return;
    }

    const usuario = this.authService.obtenerUsuario();
    if (!usuario) {
      alert('Debes iniciar sesión para reservar.');
      return;
    }

    const reserva = {
      usuarioId: usuario.id,
      horarioId: horario.id,
      fecha: new Date().toISOString(),
      estado: 'confirmada',
      promocionAplicada: horario.descuento > 0 // ✅ Marca si hubo descuento
    };

    this.canchaService.crearReserva(reserva).subscribe({
      next: () => {
        alert('Reserva realizada correctamente ✅');
        this.cargarHorarios();
      },
      error: err => {
        console.error(err);
        alert('Error al reservar ❌');
      }
    });
  }
}
