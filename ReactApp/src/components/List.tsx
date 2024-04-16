import { list } from "../signals/UserSignal";
import { useSignals } from "@preact/signals-react/runtime";

function List() {
  useSignals();

  return (
    <ol>
      {list.value.map((item) => (
        <li key={item}>{item}</li>
      ))}
    </ol>
  );
}

export default List;
