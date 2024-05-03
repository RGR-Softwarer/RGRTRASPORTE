export interface Action {
    label: string
    acao: (item: any) => void;
}