export interface Usuario {
  id?: number;
  nombre?: string;
  email: string;
  password: string;
  rol?: string; // opcional si tu backend lo genera automáticamente o puedes enviarlo tú como "user"
}
