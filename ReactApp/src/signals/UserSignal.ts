import { signal } from "@preact/signals-react";

export const count = signal(0);

export function increment() {
  count.value++;
}

export function decrement() {
  count.value--;
}
