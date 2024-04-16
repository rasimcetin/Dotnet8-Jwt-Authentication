import { effect, signal } from "@preact/signals-react";

export enum Role {
  Admin,
  User,
}

export interface User {
  id: string;
  name?: string;
  surName?: string;
  email?: string;
  username?: string;
  role: Role;
  createdAt: Date;
  updatedAt: Date;
}

export const list = signal(Array<number>(0));
export const count = signal(0);
export const userList = signal(Array<User>(0));

effect(() => {
  console.log(list.value);
});

export function increment() {
  count.value++;
  list.value = [...list.value, count.value];
}

export function decrement() {
  if (list.value.length === 0) return;
  count.value--;
  list.value.pop();
  list.value = [...list.value];
}
