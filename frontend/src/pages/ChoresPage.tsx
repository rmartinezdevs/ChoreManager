import { useEffect } from "react";
import { useChoresStore } from "../store/useChoresStore";

export default function ChoresPage() {
  const { chores, fetchChores } = useChoresStore();

  useEffect(() => {
    fetchChores();
  }, [fetchChores]);

  return (
    <div className="p-5">
      <h1 className="text-3xl font-bold">Lista de Tareas</h1>
      <ul>
        {chores.map((chore) => (
          <li key={chore.id} className="p-2 border-b">
            {chore.title} - {chore.status} - {chore.dueDate}
          </li>
        ))}
      </ul>
    </div>
  );
}
