
import { TicketType } from "./ticket-type";
export interface TicketRequest {
  price?: number;
  registrationId?: number;
  smartBraceletId?: number;
  type?: TicketType;
}
