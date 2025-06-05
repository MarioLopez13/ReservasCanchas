import { Component, OnInit } from '@angular/core';
import { CanchaService } from '../../services/cancha.service';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router'; // 👈 Se añade Router

@Component({
  selector: 'app-canchas',
  standalone: true,
  templateUrl: './canchas.component.html',
  styleUrls: ['./canchas.component.scss'],
  imports: [CommonModule, RouterModule]
})
export class CanchasComponent implements OnInit {
  canchas: any[] = [];

  constructor(
    private canchaService: CanchaService,
    private router: Router //  Se inyecta el Router
  ) {}
  // Guarda las canchas traídas del backend en el arreglo canchas
  ngOnInit() {
    this.canchaService.getCanchas().subscribe(data => {
      this.canchas = data;
    });
  }

  verHorarios(canchaId: number) {
    
    alert(`Aquí se mostrarán los horarios para la cancha con ID: ${canchaId}`);
    
  }
}
