import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Usuario } from '../../models/usuario.model';
import { Router } from '@angular/router';
import * as CryptoJS from 'crypto-js';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  imports: [ReactiveFormsModule]
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      nombre: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      rol: ['user', Validators.required] // default = user
    });
  }

  registrar() {
    if (this.registerForm.invalid) return;

    const formValues = this.registerForm.value;

    const usuario: Usuario = {
      nombre: formValues.nombre,
      email: formValues.email,
      password: CryptoJS.SHA256(formValues.password).toString(),
      rol: formValues.rol
    };

    this.authService.register(usuario).subscribe({
      next: () => {
        alert('Registro exitoso!');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error(err);
        alert('Error al registrar usuario');
      }
    });
  }
}
