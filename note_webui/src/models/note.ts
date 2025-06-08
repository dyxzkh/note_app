export interface CreateNoteDto {
  title: string
  content: string
  createdBy: number
}

export interface UpdateNoteDto {
  title?: string
  content?: string
}

export interface Note {
  id: number
  title: string
  content: string
  createdBy: number
  createdAt: string
  updatedAt: string
}

export interface ApiResponse<T> {
  success: boolean
  data?: T
  errorMessage?: string
}
