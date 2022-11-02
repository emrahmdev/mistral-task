export type PaginatedResponse<T> = {
    items: T[];
    totalPages: number;
    currentPage: number;
    hasNextPage: boolean;
}