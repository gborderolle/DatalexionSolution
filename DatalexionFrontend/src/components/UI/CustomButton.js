import { useNavigate } from "react-router-dom";
import { CCol, CButton } from "@coreui/react";
import CIcon from "@coreui/icons-react";
import classes from "./AppFooterMobileAdmin.module.css";

const CustomButton = (props) => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(props.path);
  };

  // Asegurarse de que requiredRole sea un arreglo
  const isVisible = [].concat(props.requiredRole).includes(props.userRole);

  return (
    isVisible && (
      <CCol>
        <CButton
          type="button"
          className={classes.customButton}
          color={props.color}
          onClick={handleClick}
        >
          <CIcon icon={props.icon} size="xl" />
        </CButton>
      </CCol>
    )
  );
};

export default CustomButton;
