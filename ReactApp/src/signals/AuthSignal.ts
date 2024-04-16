import { signal } from "@preact/signals-react";

export interface LoginDto {
  Username: string;
  Password: string;
}

export const token = signal<string | null>(null);
