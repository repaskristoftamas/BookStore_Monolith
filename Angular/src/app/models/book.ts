export interface Book {
    id?: number;
    title: string;
    description?: string;
    author: {
        id: number,
        name: string
    };
    genre: {
        id: number,
        name: string
    };
}
