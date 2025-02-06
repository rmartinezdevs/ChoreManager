import { create } from "zustand";

interface IChore {
  id: string;
  title: string;
  description: string;
  dueDate: string
  status: number;
  assignedUserId: string
}

interface IChoresStore {
  chores: IChore[];
  fetchChores: () => Promise<void>;
}

export const useChoresStore = create<IChoresStore>((set) => ({
    chores: [],
  
    fetchChores: async () => {
      try {
        const response = await fetch("https://localhost:7078/api/Chore");
        const jsonResponse = await response.json();
        const data = jsonResponse.data;
  
        // Validar y mapear los datos al formato de IChore
        if (Array.isArray(data)) {
          set({chores: data});
        } else {
          console.error("Formato de datos no esperado:", data);
          set({ chores: [] });
        }
      } catch (error) {
        console.error("Error fetching chores:", error);
        set({ chores: [] });
      }
    },
  }));
