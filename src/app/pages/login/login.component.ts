import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import * as CryptoJS from 'crypto-js';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [ReactiveFormsModule, RouterModule, CommonModule]
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  login() {
    if (this.loginForm.invalid) return;

    const { email, password } = this.loginForm.value;
    const hashedPassword = CryptoJS.SHA256(password).toString();

    this.authService.login(email, hashedPassword).subscribe({
      next: (res) => {
        alert(`Bienvenido, ${res.usuario.nombre}`);
        this.authService.guardarToken('temporal'); // usar JWT si está disponible
        this.authService.guardarUsuario(res.usuario); // ✅ guarda el usuario logueado
        this.router.navigate(['/canchas']);
      },
      error: (err) => {
        console.error('Error al iniciar sesión:', err);
        alert('Credenciales incorrectas');
      }
    });
  }

  irARegistro() {
    this.router.navigate(['/register']);
  }
}
